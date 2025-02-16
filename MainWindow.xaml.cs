using Diplom.Model;
using DocumentFormat.OpenXml.Spreadsheet;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Diplom
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }




        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Text;

            using (var context = new VPKSContext()) // Создаем экземпляр контекста
            {
                var user = context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
                var admin = context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
                if (login =="adm" && password=="adm")
                {

                    MessageBox.Show("Вы вошли как админ", "Успешно", MessageBoxButton.OK);
                    Main main = new Main(user);
                    this.Close();
                    main.Show();
                }
                else
                {
                    if (user != null)
                    {

                        MessageBox.Show("Вы вошли как студент", "Успешно", MessageBoxButton.OK);
                        Main main = new Main(admin);
                        this.Close();
                        main.Show();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                
                
            }
        }
    }
}