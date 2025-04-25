using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Xps.Packaging;
using Diplom.Model;

namespace Diplom
{
    public partial class DopStud : Window
    {
        public DopStud()
        {
            InitializeComponent();
            LoadDocumentList();  // Загрузить список документов
            this.WindowState = WindowState.Maximized;
        }

        // Метод для загрузки списка доступных XPS-документов
        private void LoadDocumentList()
        {
            using (var context = new VPKSContext())
            {
                // Получаем все документы с расширением .xps
                var documents = context.Documents
                    .Where(d => d.FilePath.EndsWith(".xps"))  // Фильтруем только XPS-файлы
                    .ToList();

                foreach (var doc in documents)
                {
                    // Создаем кнопку для каждого документа
                    Button documentButton = new Button
                    {
                        Content = doc.Title,       // Заголовок документа
                        Tag = doc.FilePath,        // Путь к файлу сохраняем в Tag кнопки
                        Margin = new Thickness(5),
                        Width = 300,
                        Height = 30
                    };

                    // Подписываемся на событие Click
                    documentButton.Click += DocumentButton_Click;

                    // Добавляем кнопку в панель
                    DocumentListPanel.Children.Add(documentButton);
                }
            }
        }

        // Обработчик нажатия на кнопку документа
        private void DocumentButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем путь к файлу из Tag кнопки
            string xpsPath = (sender as Button).Tag.ToString();

            if (File.Exists(xpsPath))
            {
                try
                {
                    // Загружаем XPS-документ
                    XpsDocument doc = new XpsDocument(xpsPath, FileAccess.Read);
                    documentViewer.Document = doc.GetFixedDocumentSequence();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии документа: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Документ не найден.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Открытие главного окна
           
            // Закрытие текущего окна
            this.Close();
        }
    }
}
