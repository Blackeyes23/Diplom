using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Diplom
{
    public partial class AdminAddUserWindow : Window
    {
        public AdminAddUserWindow()
        {
            InitializeComponent();
            LoadRolesAndGroups(); // Загружаем роли и группы при открытии окна
        }

        // Ограничение ввода только букв
        private void NameBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[а-яА-Яa-zA-Z]+$");
        }

        // Переключение видимости пароля
        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Visibility == Visibility.Visible)
            {
                PasswordTextBox.Text = PasswordBox.Password;
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                PasswordBox.Password = PasswordTextBox.Text;
                PasswordTextBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(NameBox.Text) ||
                string.IsNullOrWhiteSpace(SurnameBox.Text) ||
                string.IsNullOrWhiteSpace(LoginBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                RoleComboBox.SelectedItem == null ||
                GroupComboBox.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Добавление нового пользователя (пример)
            using (var context = new Diplom.Model.VPKSContext())
            {
                var newUser = new Diplom.Model.Users
                {
                    Name = NameBox.Text,
                    Surname = SurnameBox.Text,
                    Login = LoginBox.Text,
                    Password = PasswordBox.Password,
                    RoleId = (int)RoleComboBox.SelectedValue,  // Предполагается, что привязка данных настроена
                    GruppaId = (int)GroupComboBox.SelectedValue
                };

                context.Users.Add(newUser);
                context.SaveChanges();
            }

            MessageBox.Show("Пользователь успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
        private void LoadRolesAndGroups()
        {
            using (var context = new Diplom.Model.VPKSContext())
            {
                // Загружаем роли
                var roles = context.Role.ToList();
                RoleComboBox.ItemsSource = roles;
                RoleComboBox.DisplayMemberPath = "Name"; // Отображаем имя роли
                RoleComboBox.SelectedValuePath = "Id";  // Значение - ID роли

                // Загружаем группы
                var groups = context.Gruppa.ToList();
                GroupComboBox.ItemsSource = groups;
                GroupComboBox.DisplayMemberPath = "Number"; // Отображаем номер группы
                GroupComboBox.SelectedValuePath = "Id";     // Значение - ID группы
            }
        }

    }
}
