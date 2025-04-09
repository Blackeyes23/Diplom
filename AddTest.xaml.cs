using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Diplom.Model;

namespace Diplom
{
    public partial class AddTest : Window
    {
        private List<AnswerOption> answers = new List<AnswerOption>();

        public AddTest()
        {
            InitializeComponent();
        }

        // Представление одного варианта ответа
        public class AnswerOption
        {
            public string AnswerText { get; set; }
            public bool IsCorrect { get; set; }
        }

        private void AddAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (answers.Count >= 5)
            {
                MessageBox.Show("Нельзя добавить больше 5 ответов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!string.IsNullOrWhiteSpace(NewAnswerBox.Text))
            {
                answers.Add(new AnswerOption
                {
                    AnswerText = NewAnswerBox.Text,
                    IsCorrect = false
                });

                RefreshAnswersList();
                NewAnswerBox.Clear();
            }
            else
            {
                MessageBox.Show("Введите текст ответа.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CorrectAnswer_Checked(object sender, RoutedEventArgs e)
        {
            var selectedAnswer = (sender as RadioButton)?.DataContext as AnswerOption;
            if (selectedAnswer == null) return;

            foreach (var answer in answers)
                answer.IsCorrect = false;

            selectedAnswer.IsCorrect = true;
            RefreshAnswersList();
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var answer in answers)
                answer.IsCorrect = false;

            var selectedAnswer = (sender as RadioButton)?.DataContext as AnswerOption;
            if (selectedAnswer != null)
            {
                selectedAnswer.IsCorrect = true;
                RefreshAnswersList();
            }
        }

        private TestGroups currentGroup; // поле класса для хранения созданной группы

        private void SaveTest_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TestNameBox.Text))
            {
                MessageBox.Show("Введите название теста.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = new VPKSContext())
                {
                    var group = new TestGroups
                    {
                        Name = TestNameBox.Text,
                        CategoryId = 1
                    };


                    context.TestGroups.Add(group);
                    context.SaveChanges();

                    currentGroup = group; // сохранить для добавления вопросов

                    MessageBox.Show("Тест сохранён. Теперь добавьте вопросы.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении теста: {ex.InnerException?.Message ?? ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RefreshAnswersList()
        {
            AnswersListBox.ItemsSource = null;
            AnswersListBox.ItemsSource = answers;
        }

        private void SaveQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (currentGroup == null)
            {
                MessageBox.Show("Сначала сохраните тест.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(QuestionNameBox.Text))
            {
                MessageBox.Show("Введите название вопроса.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (answers.Count < 2)
            {
                MessageBox.Show("Добавьте минимум два варианта ответа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var correctAnswer = answers.FirstOrDefault(a => a.IsCorrect);
            if (correctAnswer == null)
            {
                MessageBox.Show("Не выбран правильный ответ!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var context = new VPKSContext())
                {
                    var test = new Tests
                    {
                        Question = QuestionNameBox.Text,
                        SubjectId = 1,
                        TestGroupId = currentGroup.Id,
                        CorrectAnswer = correctAnswer.AnswerText,
                        CategoryId = 1
                    };

                    context.Tests.Add(test);
                    context.SaveChanges();

                    foreach (var answer in answers)
                    {
                        var answerEntity = new Answers
                        {
                            Text = answer.AnswerText,
                            IsCorrect = answer.IsCorrect,
                            TestId = test.Id
                        };
                        context.Answers.Add(answerEntity);
                    }

                    context.SaveChanges();
                }

                MessageBox.Show("Вопрос успешно добавлен!");
                QuestionNameBox.Clear();
                answers.Clear();
                RefreshAnswersList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.InnerException?.Message ?? ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
