using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPDAssist.CCNPJ
{
    public class ClienteCNPJ : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _regiao;
        public int Regiao
        {
            get { return _regiao; }
            set
            {
                if (_regiao == value) return;
                _regiao = value;
                NotifyPropertyChanged("Regiao");
            }
        }

        private int _numero;
        public int Numero
        {
            get { return _numero; }
            set
            {
                if (_numero == value) return;
                _numero = value;
                NotifyPropertyChanged("Numero");
            }
        }

        private Pedido _pedido;
        public Pedido Pedido
        {
            get { return _pedido; }
            set
            {
                if (_pedido == value) return;
                _pedido = value;
                NotifyPropertyChanged("Pedido");
            }
        }

        private string _vendedor;
        public string Vendedor
        {
            get { return _vendedor; }
            set
            {
                if (_vendedor == value) return;
                _vendedor = value;
                NotifyPropertyChanged("Vendedor");
            }
        }

        private string _razao;
        public string Razao
        {
            get { return _razao; }
            set
            {
                if (_razao == value) return;
                _razao = value;
                NotifyPropertyChanged("Razao");
            }
        }

        private string _cnpj;
        public string CNPJ
        {
            get { return _cnpj; }
            set
            {
                if (_cnpj == value) return;
                _cnpj = value;
                NotifyPropertyChanged("CNPJ");
            }
        }

        private string _ie;
        public string IE
        {
            get { return _ie; }
            set
            {
                if (_ie == value) return;
                _ie = value;
                NotifyPropertyChanged("IE");
            }
        }

        private string _situacao;
        public string Situacao
        {
            get { return _situacao; }
            set
            {
                if (_situacao == value) return;
                _situacao = value;
                NotifyPropertyChanged("Situacao");
            }
        }

        private cadCliente _detalhes;
        public cadCliente Detalhes
        {
            get { return _detalhes; }
            set
            {
                if (_detalhes == value) return;
                _detalhes = value;
                NotifyPropertyChanged("Detalhes");
            }
        }
    }
}
