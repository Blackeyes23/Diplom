using System;
using System.Windows;

namespace Diplom
{
    public partial class CompilerWindow : Window
    {
        public CompilerWindow()
        {
            InitializeComponent();
        }

        // Обновляем номер текущей строки
        private void UpdateCurrentLine()
        {
            // Получаем позицию курсора (начало выделения)
            int caretIndex = CodeTextBox.CaretIndex;
            int lineNumber = CodeTextBox.GetLineIndexFromCharacterIndex(caretIndex) + 1;

            // Прибавляем к номеру строки 6 из-за скрытого блока
            lineNumber += 6;

            // Отображаем текущую строку в отдельном поле
            CurrentLineTextBox.Text = lineNumber.ToString();
        }

        // Обработчик изменения текста
        private void CodeTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            UpdateCurrentLine();
        }

        // Обработчик изменения позиции курсора
        private void CodeTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateCurrentLine();
        }

        // Обработчик клика на кнопку "Компилировать и выполнить"
        private void CompileAndRun_Click(object sender, RoutedEventArgs e)
        {
            string code = CodeTextBox.Text;

            // Проверка на пустое поле
            if (string.IsNullOrWhiteSpace(code))
            {
                OutputTextBox.Text = "Ошибка: поле ввода пустое!";
                return;
            }

            var compiler = new Compiler();
            string result = compiler.CompileAndExecuteCode(code);
            OutputTextBox.Text = result;
        }

        // Обработчик клика на кнопку "Очистить"
        private void ClearCode_Click(object sender, RoutedEventArgs e)
        {
            CodeTextBox.Clear();
            OutputTextBox.Clear();
            CurrentLineTextBox.Text = "0";
        }
    }
}
