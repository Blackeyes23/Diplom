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
            LoadDocumentList();  
            this.WindowState = WindowState.Maximized;
        }

        private void LoadDocumentList()
        {
            using (var context = new VPKSContext())
            {
                var documents = context.Documents
                    .Where(d => d.FilePath.EndsWith(".xps")) 
                    .ToList();

                foreach (var doc in documents)
                {
                    Button documentButton = new Button
                    {
                        Content = doc.Title,       
                        Tag = doc.FilePath,       
                        Margin = new Thickness(5),
                        Width = 300,
                        Height = 30
                    };

                    documentButton.Click += DocumentButton_Click;

                    DocumentListPanel.Children.Add(documentButton);
                }
            }
        }

        private void DocumentButton_Click(object sender, RoutedEventArgs e)
        {
            string xpsPath = (sender as Button).Tag.ToString();

            if (File.Exists(xpsPath))
            {
                try
                {
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
            
            this.Close();
        }
    }
}
