using System;
using System.Windows;
using mshtml;
using System.Windows.Navigation;

namespace CPDAssist.CCNPJ
{
    /// <summary>
    /// Lógica interna para conSintegra.xaml
    /// </summary>
    public partial class conSintegra : Window
    {
        public string docCons = "";
        public conSintegra(string cnpj)
        {
            InitializeComponent();
            webBrowser1.Navigate(new Uri("http://consultasintegra.fazenda.mg.gov.br/sintegra/"));
            docCons = cnpj;
        }

        HTMLDocument doc;

        public HTMLDocument Hdoc
        {
            get
            {
                return doc;
            }

            set
            {
                doc = value;
            }
        }

        private void webBrowser1_LoadCompleted(object sender, NavigationEventArgs e)
        {
            Hdoc = (HTMLDocument)webBrowser1.Document;
            HTMLInputElement txtcnpj = Hdoc.all.item("filtro", 0);
            HTMLSelectElement cboDoc = Hdoc.all.item("identificadorCmbOpcao", 0);

            cboDoc.setAttribute("value", "2");
            txtcnpj.value = docCons;
        }
    }
}
