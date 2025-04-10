    using Diplom.Model;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Windows;

    namespace Diplom
    {
        public partial class MainWindow : Window
        {
            public MainWindow()
            {
                InitializeComponent();
                this.WindowState=WindowState.Maximized;
            }

            private void Button_Click(object sender, RoutedEventArgs e)
            {
                string login = Login.Text;
            string password = Password.Password;

            using (var context = new VPKSContext()) // Создаем экземпляр контекста
                {
                    var user = context.Users
                        .Include(u => u.Role) // Загружаем связанную роль
                        .FirstOrDefault(u => u.Login == login && u.Password == password);

                    if (user != null)
                    {
                        MessageBox.Show($"Вы вошли как {user.Role.Name}", "Успешно", MessageBoxButton.OK);

                        // Открываем соответствующее окно по роли
                        switch (user.RoleId)
                        {
                            case 1:
                                new Main(user).Show();
                                break;
                            case 2:
                                new Admin(user).Show();
                                break;
                            case 3:
                                new Prepod(user).Show();
                                break;
                            default:
                                MessageBox.Show("Неизвестная роль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                        }

                        this.Close(); // Закрываем окно авторизации
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
