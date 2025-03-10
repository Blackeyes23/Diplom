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
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (_currentUser != null)
            {
                new Praktika(_currentUser).Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка: пользователь не определён!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadUserResults(Model.Users user)
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

    }
}
