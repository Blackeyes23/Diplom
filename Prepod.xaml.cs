using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Diplom
{
    /// <summary>
    /// Логика взаимодействия для Prepod.xaml
    /// </summary>
    public partial class Prepod : Window
    {
        public Prepod(Model.Users user)
        {
            InitializeComponent();
            DisplayUserInfo(user);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // Проверяем, открыто ли уже окно Teor
            foreach (Window window in Application.Current.Windows)
            {
                if (window is Teor)
                {
                    window.Activate(); // Переключаемся на уже открытое окно
                    return; // Прекращаем выполнение метода
                }
            }

            // Если окна нет, создаем и открываем его
            Teor teor = new Teor();
            teor.Show();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is AddTest)
                {
                    window.Activate(); // Переключаемся на уже открытое окно
                    return; // Прекращаем выполнение метода
                }
            }

            AddTest addTest = new AddTest(); 
            addTest.Show();

        }
        private void DisplayUserInfo(Diplom.Model.Users user)
        {
            NameText.Text = user.Name;
            SurnameText.Text = user.Surname;
            GroupText.Text = user.Gruppa != null ? user.Gruppa.Number.ToString() : "Не указано";
            RoleText.Text = user.Role != null ? user.Role.Name.ToString() : "Не указано";
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

    }
}
