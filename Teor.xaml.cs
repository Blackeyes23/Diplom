using Diplom.Model;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace Diplom
{
    public partial class Teor : Window
    {
        private string uploadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uploads");

        public Teor()
        {
            InitializeComponent();
            Directory.CreateDirectory(uploadPath); // Создаем папку, если нет
        }

        private void LoadDocxButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Документы Word (*.docx)|*.docx"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = Path.GetFileName(openFileDialog.FileName);
                string destinationPath = Path.Combine(uploadPath, fileName);
                File.Copy(openFileDialog.FileName, destinationPath, true);

                // Сохранение пути в базу данных
                SaveFilePathToDatabase(fileName, destinationPath);

                MessageBox.Show("Файл успешно загружен!");
            }
        }

        private void SaveFilePathToDatabase(string title, string filePath)
        {
            using (var context = new VPKSContext()) // Замени на свой DbContext
            {
                var document = new Documents // Соответствует классу из Model
                {
                    Title = title,
                    FilePath = filePath,
                        UploaderId = 4 // ID существующего пользователя в таблице Users

                };

                context.Documents.Add(document);
                context.SaveChanges();
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
