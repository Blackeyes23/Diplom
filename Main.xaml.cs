using System;
using System.Linq;
using System.Windows;
using Diplom.Model;
using DocumentFormat.OpenXml.Spreadsheet;

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
                    var results = context.TestResults
                        .Where(r => r.UserId == user.Id)
                        .Select(r => new
                        {
                            TestName = r.Test.Question,
                            Score = r.Score,
                            Correct = r.IsCorrect ? "✔" : "✘"
                        })
                        .ToList();

                    ResultsList.ItemsSource = results;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке результатов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



    }
}
