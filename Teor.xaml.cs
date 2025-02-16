using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;

namespace Diplom
{
    public partial class Teor : Window
    {
        public Teor()
        {
            InitializeComponent();
        }

        private void LoadDocxButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Документы Word (*.docx)|*.docx",
                Title = "Выберите файл"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    string text = ExtractTextFromDocx(filePath);
                    DisplayText(text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки документа:\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string ExtractTextFromDocx(string filePath)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, false))
            {
                Body body = doc.MainDocumentPart.Document.Body;
                return body.InnerText;
            }
        }

        private void DisplayText(string text)
        {
            DocxTextBox.Document.Blocks.Clear();
            DocxTextBox.Document.Blocks.Add(new System.Windows.Documents.Paragraph(new System.Windows.Documents.Run(text)));
        }
    }
}
