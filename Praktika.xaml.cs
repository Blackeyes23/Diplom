using System;
using System.Collections.Generic;
 using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Diplom.Model;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;

namespace Diplom
{
    public partial class Praktika : Window
    {
        private double _totalSeconds;
        private readonly VPKSContext _context;
        private Tests _currentTest;
        private List<Tests> _groupQuestions;
        private int _currentQuestionIndex;
        private int _score;
        private int _userId;
        private Model.Users _currentUser;
        private DispatcherTimer _timer;
        private TimeSpan _timeLeft;
        private bool _warningShown = false;


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
                StartTimer(_groupQuestions.Count); // Перенести сюда
            }

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timeLeft = _timeLeft.Subtract(TimeSpan.FromSeconds(1));
            TimerText.Text = $"Время: {_timeLeft.Minutes:D2}:{_timeLeft.Seconds:D2}";

            double secondsLeft = _timeLeft.TotalSeconds;
            TimeProgressBar.Value = secondsLeft;

            // Анимация счётчика — мигание при малом времени
            if (secondsLeft <= _totalSeconds * 0.25)
            {
                TimerText.Foreground = TimerText.Foreground == Brushes.Red ? Brushes.Black : Brushes.Red;
            }

            // Предупреждение за 75% времени
            double percentPassed = 1 - (secondsLeft / _totalSeconds);
            if (!_warningShown && percentPassed >= 0.75)
            {
                _warningShown = true;
                MessageBox.Show("Осталось совсем немного времени!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (_timeLeft <= TimeSpan.Zero)
            {
                _timer.Stop();
                MessageBox.Show("Время вышло! Тест завершён автоматически.", "Время истекло", MessageBoxButton.OK, MessageBoxImage.Information);
                FinishTest();
            }
        }



        private void StartTimer(int questionCount)
        {
            _timeLeft = TimeSpan.FromMinutes(questionCount); // 1 минута на вопрос
            _totalSeconds = _timeLeft.TotalSeconds;
            _warningShown = false;

            TimeProgressBar.Maximum = _totalSeconds;
            TimeProgressBar.Value = _totalSeconds;

            _timer?.Stop();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
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
            _timer?.Stop();

            MessageBox.Show($"Тест завершён! Ваш результат: {_score} из {_groupQuestions.Count}", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);

            // Очистка
            QuestionText.Text = "";
            AnswersList.Items.Clear();
            ResultText.Text = "";
            _groupQuestions = null;

            TimerText.Text = "";
            TimerText.Foreground = Brushes.Black;
            TimeProgressBar.Value = 0;

            if (MessageBox.Show("Хотите пройти другой тест?", "Выбор", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                TestSelectionComboBox.SelectedIndex = -1;
            }
            else
            {
                this.Close();
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
