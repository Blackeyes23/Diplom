using Diplom.Model;
using System.Windows;

namespace Diplom
{
    public partial class Admin : Window
    {
        public Admin(Users user)
        {
            InitializeComponent();
            DisplayUserInfo(user);
            this.WindowState = WindowState.Maximized;
        }

        private void DisplayUserInfo(Users user)
        {
            NameText.Text = user.Name;
            SurnameText.Text = user.Surname;
            GroupText.Text = user.Gruppa != null ? user.Gruppa.Number.ToString() : "Не указано";
            RoleText.Text = user.Role != null ? user.Role.Name : "Не указано"; 
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is AdminAddUserWindow)
                {
                    window.Activate(); 
                    return; 
                }
            }
            AdminAddUserWindow addUserWindow = new AdminAddUserWindow();
            addUserWindow.ShowDialog();
        }
    }
}
