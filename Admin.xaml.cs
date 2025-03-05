﻿using Diplom.Model;
using System.Windows;

namespace Diplom
{
    public partial class Admin : Window
    {
        public Admin(Users user)
        {
            InitializeComponent();
            DisplayUserInfo(user);
        }

        private void DisplayUserInfo(Users user)
        {
            NameText.Text = user.Name;
            SurnameText.Text = user.Surname;
            GroupText.Text = user.Gruppa != null ? user.Gruppa.Number.ToString() : "Не указано";
            RoleText.Text = user.Role != null ? user.Role.Name : "Не указано"; // Исправлено
        }

        private void OpenAddUserWindow(object sender, RoutedEventArgs e)
        {
            AdminAddUserWindow addUserWindow = new AdminAddUserWindow();
            addUserWindow.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
