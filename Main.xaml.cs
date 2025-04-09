using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Diplom.Model;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Core;

namespace Diplom
{
    public partial class Main : Window
    {
        private Model.Users _currentUser;

        public Main(Diplom.Model.Users user)
        {
            InitializeComponent();
            _currentUser = user;
            DisplayUserInfo(user);
            LoadUserResults(user); // Добавьте этот вызов
            this.WindowState = WindowState.Maximized;

        }



        private void DisplayUserInfo(Diplom.Model.Users user)
        {
            NameText.Text = user.Name;
            SurnameText.Text = user.Surname;
            GroupText.Text = user.Gruppa != null ? user.Gruppa.Number.ToString() : "Не указано";
            RoleText.Text = user.Role != null ? user.Role.Name.ToString() : "Не указано";
        }



        



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // Проверяем, открыто ли уже окно Teor
            foreach (Window window in Application.Current.Windows)
            {
                if (window is TeorStud)
                {
                    window.Activate(); // Переключаемся на уже открытое окно
                    return; // Прекращаем выполнение метода
                }
            }


            TeorStud teorStud = new TeorStud();
            teorStud.Show();
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Открытие главного окна
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            // Закрытие всех окон, кроме главного
            foreach (Window window in Application.Current.Windows)
            {
                if (window != mainWindow)
                {
                    window.Close();
                }
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            // Проверяем, открыто ли уже окно Teor
            foreach (Window window in Application.Current.Windows)
            {
                if (window is Praktika)
                {
                    window.Activate(); // Переключаемся на уже открытое окно
                    return; // Прекращаем выполнение метода
                }
            }
            if (_currentUser != null)
            {
                new Praktika(_currentUser).Show();
               
            }
            else
            {
                MessageBox.Show("Ошибка: пользователь не определён!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadUserResults(Diplom.Model.Users user)
        {
            try
            {
                using (var context = new VPKSContext())
                {
                    // Загружаем все результаты пользователя и связанные тесты + группы
                    var results = context.TestResults
                        .Include(r => r.Test)
                            .ThenInclude(t => t.TestGroup)
                        .Where(r => r.UserId == user.Id)
                        .ToList(); // загружаем в память

                    // Группируем уже в памяти
                    var groupedResults = results
                        .GroupBy(r => r.TestId)
                        .ToList();

                    var resultList = new List<object>();

                    foreach (var group in groupedResults)
                    {
                        int correctAnswers = group.Count(r => r.IsCorrect);
                        int totalQuestions = group.Count();
                        int score = CalculateScore(correctAnswers, totalQuestions);

                        var first = group.FirstOrDefault();
                        if (first != null && (first.Score == null || first.Score != score))
                        {
                            first.Score = score;
                            context.Update(first);
                            context.SaveChanges();
                        }

                        var testName = first?.Test?.TestGroup?.Name ?? "Неизвестный тест";

                        resultList.Add(new
                        {
                            TestName = testName,
                            ScoreDisplay = $"Оценка: {score}"
                        });
                    }

                    ResultsList.ItemsSource = resultList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке результатов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private string CalculateGrade(int correct, int total)
        {
            if (total == 0) return "Н/Д";
            double percentage = (double)correct / total;

            if (percentage >= 0.9) return "5";
            else if (percentage >= 0.75) return "4";
            else if (percentage >= 0.5) return "3";
            else return "2";
        }

        private int CalculateScore(int correct, int total)
        {
            if (total == 0) return 0;
            double percentage = (double)correct / total;

            if (percentage >= 0.9) return 5;
            else if (percentage >= 0.75) return 4;
            else if (percentage >= 0.5) return 3;
            else return 2;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // Проверяем, открыто ли уже окно Teor
            foreach (Window window in Application.Current.Windows)
            {
                if (window is DopStud)
                {
                    window.Activate(); // Переключаемся на уже открытое окно
                    return; // Прекращаем выполнение метода
                }
            }


            DopStud dopStud = new DopStud();
            dopStud.Show();
        }
    }
}
