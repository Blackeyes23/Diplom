using System;
using System.Linq;
using System.Windows;
using Diplom.Model;
using Microsoft.EntityFrameworkCore;

namespace Diplom
{
    public partial class StudentResultsWindow : Window
    {
        public StudentResultsWindow()
        {
            InitializeComponent();
            LoadGroups();
        }

        private void LoadGroups()
        {
            using (var context = new VPKSContext())
            {
                // Получаем список групп из базы данных
                var groups = context.Gruppa.ToList(); // Предположим, что группа хранится в таблице Gruppas
                GroupComboBox.ItemsSource = groups;
                GroupComboBox.DisplayMemberPath = "Number"; // Показать номер группы
                GroupComboBox.SelectedValuePath = "Id"; // Использовать Id группы как значение
            }
        }


        private void GroupComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GroupComboBox.SelectedValue is int selectedGroupId)
            {
                LoadStudentResults(selectedGroupId);
            }
        }

        private void LoadStudentResults(int groupId)
        {
            using (var context = new VPKSContext())
            {
                var results = context.TestResults
                    .Include(r => r.Test)
                        .ThenInclude(t => t.TestGroup)
                    .Include(r => r.User)
                        .ThenInclude(u => u.Gruppa)
                    .Where(r => r.User.GruppaId == groupId)
                    .ToList()
                    .GroupBy(r => new { r.UserId, r.TestId })
                    .Select(g =>
                    {
                        var first = g.FirstOrDefault();
                        if (first == null)
                            return null;

                        return new
                        {
                            Surname = first.User?.Surname ?? "—",
                            Name = first.User?.Name ?? "—",
                            TestName = first.Test?.TestGroup?.Name ?? "Неизвестно",
                            Score = first.Score // Убираем ??, так как Score не может быть null
                        };
                    })
                    .Where(r => r != null)  // Отбрасываем null значения
                    .OrderBy(r => r.Surname)  // Сортировка по фамилии
                    .ThenByDescending(r => r.Score)  // Сортировка по оценке, по убыванию
                    .ToList();

                ResultsListView.ItemsSource = results;
            }
        }


    }
}
