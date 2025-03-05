using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Diplom.Model;

namespace Diplom
{
    /// <summary>
    /// Логика взаимодействия для TeorStud.xaml
    /// </summary>
    public partial class TeorStud : Window
    {
        public TeorStud()
        {
            InitializeComponent();
            LoadTheoryList();
        }
        private void LoadTheoryList()
        {
            using (var context = new VPKSContext()) // Замени на свой DbContext
            {
                List<Documents> documents = context.Documents.ToList();
                foreach (var doc in documents)
                {
                    Button topicButton = new Button
                    {
                        Content = doc.Title,
                        Tag = doc.FilePath,
                        Margin = new Thickness(5),
                        Width = 300,
                        Height = 30
                    };

                    topicButton.Click += TopicButton_Click;
                    TheoryListPanel.Children.Add(topicButton);
                }
            }
        }

        private void TopicButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                string filePath = btn.Tag.ToString();
                OpenDocx(filePath);
            }
        }

        private void OpenDocx(string filePath)
        {
            if (File.Exists(filePath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show("Файл не найден!");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}