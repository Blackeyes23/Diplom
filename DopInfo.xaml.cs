using System;
using System.IO;
using System.Windows;
using System.Windows.Xps.Packaging;
using Microsoft.Win32;
using Diplom.Model;
using System.Windows.Documents;
using System.Windows.Xps;
using Word = Microsoft.Office.Interop.Word;

namespace Diplom
{
    public partial class DopInfo : Window
    {
        private string uploadPath = @"C:\DiplomDocs"; // Папка для документов

        public DopInfo()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        // Загрузка .docx, конвертация в .xps и отображение
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Документы Word (*.docx)|*.docx"
            };

            if (ofd.ShowDialog() == true)
            {
                string xpsPath = ConvertDocxToXps(ofd.FileName);
                if (xpsPath != null)
                {
                    XpsDocument doc = new XpsDocument(xpsPath, FileAccess.Read);
                    documentViewer.Document = doc.GetFixedDocumentSequence();

                    SaveFilePathToDatabase(ofd.FileName, xpsPath);
                }
                else
                {
                    MessageBox.Show("Ошибка конвертации файла.");
                }
            }
        }

        // Конвертация .docx -> .xps
        private string ConvertDocxToXps(string docxPath)
        {
            string xpsPath = Path.ChangeExtension(docxPath, ".xps");

            Word.Application wordApp = new Word.Application();
            Word.Document wordDoc = null;

            try
            {
                wordDoc = wordApp.Documents.Open(docxPath);
                wordDoc.ExportAsFixedFormat(xpsPath, Word.WdExportFormat.wdExportFormatXPS);
                return xpsPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
                return null;
            }
            finally
            {
                wordDoc?.Close(false);
                wordApp.Quit();
            }
        }

        // Сохранение пути файла в базу данных
        private void SaveFilePathToDatabase(string originalPath, string xpsPath)
        {
            using (var context = new VPKSContext())
            {
                var document = new Documents
                {
                    Title = Path.GetFileName(originalPath),
                    FilePath = xpsPath,
                    UploadDate = DateTime.Now,
                    UploaderId = 4 // ID преподавателя
                };

                context.Documents.Add(document);
                context.SaveChanges();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
