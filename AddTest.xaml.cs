using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Diplom.Model; // Убедись, что у тебя есть модель для работы с БД

namespace Diplom
{
    public partial class AddTest : Window
    {
        // Список объектов, представляющих каждый ответ с флагом правильности
        private List<AnswerOption> answers = new List<AnswerOption>();

        public AddTest()
        {
            InitializeComponent();
        }

        // Класс для представления одного варианта ответа
        public class AnswerOption
        {
            public string AnswerText { get; set; }
            public bool IsCorrect { get; set; }
        }

        // Добавление варианта ответа
        private void AddAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewAnswerBox.Text))
            {
                // Добавляем новый вариант с флажком IsCorrect = false
                answers.Add(new AnswerOption
                {
                    AnswerText = NewAnswerBox.Text,
                    IsCorrect = false
                });

                // Обновляем ListBox с новыми ответами
                AnswersListBox.ItemsSource = null;
                AnswersListBox.ItemsSource = answers;
                NewAnswerBox.Clear();
            }
        }

        // Обработчик клика по RadioButton, чтобы пометить выбранный ответ как правильный
        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            // Сбрасываем флаг правильного ответа для всех вариантов
            foreach (var answer in answers)
            {
                answer.IsCorrect = false;
            }

            // Устанавливаем флаг IsCorrect в true для выбранного ответа
            var selectedAnswer = (sender as RadioButton).Content.ToString();
            var answerOption = answers.FirstOrDefault(a => a.AnswerText == selectedAnswer);
            if (answerOption != null)
            {
                answerOption.IsCorrect = true;
            }
        }

        // Сохранение теста в БД
        private void SaveTest_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TestNameBox.Text) || answers.Count < 2)
            {
                MessageBox.Show("Введите название теста и добавьте минимум два варианта ответа.");
                return;
            }

            using (var context = new VPKSContext())
            {
                var test = new Tests
                {
                    Question = TestNameBox.Text,
                    SubjectId = 1 // Пока временно
                };

                context.Tests.Add(test);
                context.SaveChanges(); // ✅ Сначала сохраняем тест, чтобы получить его ID

                // Добавляем ответы в БД
                foreach (var answer in answers)
                {
                    var answerEntity = new Answers
                    {
                        Text = answer.AnswerText,
                        IsCorrect = answer.IsCorrect, // Применяем флажок правильного ответа
                        TestId = test.Id
                    };
                    context.Answers.Add(answerEntity);
                }

                context.SaveChanges();
            }

            MessageBox.Show("Тест успешно добавлен!");
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


}