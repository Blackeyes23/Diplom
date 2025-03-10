using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Diplom.Model; // Убедись, что у тебя есть модель для работы с БД

namespace Diplom
{
    public partial class AddTest : Window
    {
        private List<string> answers = new List<string>(); // Список ответов

        public AddTest()
        {
            InitializeComponent();
        }

        // Добавление варианта ответа
        private void AddAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(NewAnswerBox.Text))
            {
                answers.Add(NewAnswerBox.Text);
                AnswersListBox.Items.Add(NewAnswerBox.Text);
                NewAnswerBox.Clear();
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

                foreach (var answerText in answers)
                {
                    var answer = new Answers
                    {
                        Text = answerText,
                        IsCorrect = (answerText == answers[0]), // ✅ Делаем первый ответ правильным
                        TestId = test.Id
                    };
                    context.Answers.Add(answer);
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
