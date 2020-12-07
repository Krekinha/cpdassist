using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace CPDAssist.MDFe
{
    /// <summary>
    /// Lógica interna para InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public InputDialog(string question, [Optional]string defaultAnswer, [Optional]string Err)
        {
            InitializeComponent();
            lblQuestion.Content = question;
            txtAnswer.Text = defaultAnswer;
            lblErr.Content = Err;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string Answer => txtAnswer.Text;

    }
}
