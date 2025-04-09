using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Diplom.Model;
using Microsoft.EntityFrameworkCore;

namespace Diplom
{
    public partial class Praktika : Window
    {
        private readonly VPKSContext _context;
        private Tests _currentTest;
        private List<Tests> _groupQuestions;
        private int _currentQuestionIndex;
        private int _score;
        private int _userId;
        private Model.Users _currentUser;

        public Praktika(Model.Users user)
        {
            InitializeComponent();
            _currentUser = user;
            _userId = user.Id;
            LoadTestList();
        }

        private void LoadTestList()
        {
            using (var context = new VPKSContext())
            {
                var groupsWithQuestions = context.TestGroups
                    .Where(g => context.Tests.Any(t => t.TestGroupId == g.Id))
                    .ToList();

                if (groupsWithQuestions.Count == 0)
                {
                    MessageBox.Show("Нет доступных тестов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                TestSelectionComboBox.ItemsSource = groupsWithQuestions;
                TestSelectionComboBox.DisplayMemberPath = "Name";
                TestSelectionComboBox.SelectedValuePath = "Id";
            }
        }
    


        private void TestSelectionComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TestSelectionComboBox.SelectedItem is TestGroups selectedGroup)
            {
                using (var context = new VPKSContext())
                {
                    _groupQuestions = context.Tests
                        .Where(t => t.TestGroupId == selectedGroup.Id)
                        .Include(t => t.Answers)
                        .ToList();
                }

                _currentQuestionIndex = 0;
                _score = 0;

                if (_groupQuestions.Count == 0)
                {
                    MessageBox.Show("В выбранном тесте нет вопросов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                ShowCurrentQuestion();
            }
        }

        private void ShowCurrentQuestion()
        {
            if (_currentQuestionIndex >= _groupQuestions.Count)
            {
                FinishTest();
                return;
            }

            _currentTest = _groupQuestions[_currentQuestionIndex];

            QuestionText.Text = _currentTest.Question;
            AnswersList.Items.Clear();

            foreach (var answer in _currentTest.Answers.OrderBy(a => Guid.NewGuid()))
            {
                AnswersList.Items.Add(answer.Text);
            }

            ResultText.Text = "";
        }


        private void FinishTest()
        {
            MessageBox.Show($"Тест завершён! Ваш результат: {_score} из {_groupQuestions.Count}", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);

            // Очистка
            QuestionText.Text = "";
            AnswersList.Items.Clear();
            ResultText.Text = "";
            _groupQuestions = null;

            if (MessageBox.Show("Хотите пройти другой тест?", "Выбор", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                TestSelectionComboBox.SelectedIndex = -1;
            }
            else
            {
                this.Close(); // или вернуться в главное окно
            }
        }






        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTest == null || AnswersList.SelectedItem == null)
            {
                MessageBox.Show("Выберите ответ!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string selectedAnswer = AnswersList.SelectedItem.ToString();
            var correctAnswer = _currentTest.Answers.FirstOrDefault(a => a.IsCorrect);

            bool isCorrect = selectedAnswer == correctAnswer?.Text;

            if (isCorrect) _score++;

            using (var context = new VPKSContext())
            {
                var result = new TestResults
                {
                    UserId = _userId,
                    TestId = _currentTest.Id,
                    GivenAnswer = selectedAnswer,
                    IsCorrect = isCorrect,
                    Score = isCorrect ? 1 : 0
                };

                context.TestResults.Add(result);
                context.SaveChanges();
            }

            ResultText.Text = isCorrect ? "Правильно!" : $"Неправильно! Верный ответ: {correctAnswer?.Text}";

            _currentQuestionIndex++;
            ShowCurrentQuestion();
        }

    }

}
