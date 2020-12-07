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

namespace CPDAssist.MDFe
{
    /// <summary>
    /// Interação lógica para ucChave.xam
    /// </summary>
    public partial class ucChave : UserControl
    {
        public ucChave()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ChaveTypeProperty = DependencyProperty.Register(
            "Chave", typeof(string), typeof(ucChave), new PropertyMetadata(default(string)));

        public string Chave
        {
            get { return (string) GetValue(ChaveTypeProperty); }
            set { SetValue(ChaveTypeProperty, value); }
        }

        public string ChaveOld
        {
            get { return txtChave.Text; }
            set { txtChave.Text = value; }
        }

        private void cmdChaveToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtChave.Text);
        }
    }
}
