using System;
using System.Linq;
using ClosedXML.Excel;
using Microsoft.Win32;
using System.IO;
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

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            if (GroupComboBox.SelectedItem is Gruppa selectedGroup)
            {
                var items = ResultsListView.ItemsSource;
                if (items == null) return;

                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Результаты");

                // Заголовки
                worksheet.Cell(1, 1).Value = "Фамилия";
                worksheet.Cell(1, 2).Value = "Имя";
                worksheet.Cell(1, 3).Value = "Тест";
                worksheet.Cell(1, 4).Value = "Оценка";

                int row = 2;
                foreach (dynamic item in items)
                {
                    worksheet.Cell(row, 1).Value = item.Surname;
                    worksheet.Cell(row, 2).Value = item.Name;
                    worksheet.Cell(row, 3).Value = item.TestName;
                    worksheet.Cell(row, 4).Value = item.Score;
                    row++;
                }

                // Автоподбор ширины столбцов
                worksheet.Columns().AdjustToContents();

                // Сохранение файла
                string fileName = $"Аттестация {selectedGroup.Number}.xlsx";
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = fileName,
                    Filter = "Excel файлы (*.xlsx)|*.xlsx"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Экспорт завершен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                }
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
