using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Diplom.Model; // Убедись, что у тебя есть модель для работы с БД
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;

namespace Diplom
{
    /// <summary>
    /// Логика взаимодействия для Praktika.xaml
    /// </summary>
    public partial class Praktika : Window
    {
        private readonly VPKSContext _context;
        private List<Tests> _tests;
        private int _currentTestIndex;
        private int _score;
        private int _userId; // ID текущего пользователя
        private Model.Users _currentUser;

        public Praktika(Model.Users user)
        {
            InitializeComponent();
            _currentUser = user;
            _userId = user.Id; 
            LoadTests();

        }



        private void LoadTests()
        {
            using (var context = new VPKSContext())
            {
                _tests = context.Tests.Include(t => t.Answers).ToList();

                if (_tests.Count == 0)
                {
                    MessageBox.Show("В базе данных нет тестов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _currentTestIndex = 0;
                _score = 0;
                ShowTest();
            }
        }





        private void ShowTest()
        {
            if (_currentTestIndex >= _tests.Count)
            {
                ShowFinalScore();
                return;
            }

            var test = _tests[_currentTestIndex];

            QuestionText.Text = test.Question;
            AnswersList.Items.Clear();

            var answers = test.Answers.OrderBy(a => Guid.NewGuid()).ToList(); // ✅ Перемешиваем

            foreach (var answer in answers)
            {
                AnswersList.Items.Add(answer.Text);
            }
        }




        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (AnswersList.SelectedItem == null)
            {
                MessageBox.Show("Выберите ответ!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var test = _tests[_currentTestIndex];
            string selectedAnswer = AnswersList.SelectedItem.ToString();
            bool isCorrect = test.Answers.Any(a => a.Text == selectedAnswer && a.IsCorrect);

            if (isCorrect) _score++;

            using (var context = new VPKSContext())
            {
                var result = new TestResults
                {
                    UserId = _userId,
                    TestId = test.Id,
                    GivenAnswer = selectedAnswer,
                    IsCorrect = isCorrect,
                    Score = isCorrect ? 1 : 0
                };

                context.TestResults.Add(result);
                context.SaveChanges();
            }

            ResultText.Text = isCorrect ? "Правильно!" : $"Неправильно! Верный ответ: {test.Answers.First(a => a.IsCorrect).Text}";

            _currentTestIndex++;
            ShowTest();
        }


        private void ShowFinalScore()
        {
            double percentage = (double)_score / _tests.Count * 100;
            int grade = percentage >= 90 ? 5 : percentage >= 75 ? 4 : percentage >= 50 ? 3 : 2;

            MessageBox.Show($"Тест завершён!\nПравильных ответов: {_score}/{_tests.Count}\nВаша оценка: {grade}",
                "Результат", MessageBoxButton.OK, MessageBoxImage.Information);

            Close();
        }


    }
}