using System.Windows;
using Diplom.Model;
namespace Diplom
{
    public partial class Main : Window
    {
        public Main(Diplom.Model.Users user)
        {
            InitializeComponent();
            DisplayUserInfo(user);
        }

        private void DisplayUserInfo(Diplom.Model.Users user)
        {
            NameText.Text = user.Name;
            SurnameText.Text = user.Surname;
            GroupText.Text = user.Gruppa != null ? user.Gruppa.Number.ToString() : "Не указано";
            RoleText.Text = user.Role != null ? user.Role.Name.ToString() : "Не указано";
        }

        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Praktika praktika = new Praktika();
            praktika.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TeorStud teorStud= new TeorStud();
            teorStud.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
