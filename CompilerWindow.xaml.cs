using System.Windows;

namespace Diplom
{
    public partial class CompilerWindow : Window
    {
        public CompilerWindow()
        {
            InitializeComponent();
        }

        private void CompileAndRun_Click(object sender, RoutedEventArgs e)
        {
            string code = CodeTextBox.Text;
            var compiler = new Compiler();
            string result = compiler.CompileAndExecuteCode(code);
            OutputTextBox.Text = result;
        }
    }
}
