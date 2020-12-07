using System;
using System.ComponentModel;

namespace CPDAssist.MDFe
{
    public class NFMDFe : INotifyPropertyChanged, IEditableObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private NFMDFe temp_NFMDFe = null;
        private bool m_Editing = false;

        private string _razao;
        private string _cnpjEmitente; 
        private string _origem;
        private string _ufo;
        private string _destino;
        private string _ufd;
        private string _placa;
        private string _numNf;
        private string _codUF;
        private string _natureza;
        private ChavesNfe _chaves;
        private decimal _peso;
        private decimal _valor;
        private decimal _valBoleto;
        private decimal _valICMSP;
        private decimal _valICMSST;
        private DateTime _emissao;
        private string _tipoEmissao;
        private Veiculo _veiculo;
        private string _pathNf;
        private bool _isdelete = false;

        public string Razao
        {
            get { return _razao; }
            set
            {
                if (value == _razao) return;
                _razao = value;
                NotifyPropertyChanged("Razao");
            }
        }
        public string CNPJEmitente
        {
            get { return _cnpjEmitente; }
            set
            {
                if (value == _cnpjEmitente) return;
                _cnpjEmitente = value;
                NotifyPropertyChanged("CNPJEmitente");
            }
        }
        public string Origem
        {
            get { return _origem; }
            set
            {
                if (value == _origem) return;
                _origem = value;
                NotifyPropertyChanged("Origem");
            }
        }
        public string UFO
        {
            get { return _ufo; }
            set
            {
                if (value == _ufo) return;
                _ufo= value;
                NotifyPropertyChanged("UFO");
            }
        }
        public string Destino
        {
            get { return _destino; }
            set
            {
                if (value == _destino) return;
                _destino = value;
                NotifyPropertyChanged("Destino");
            }
        }
        public string UFD
        {
            get { return _ufd; }
            set
            {
                if (value == _ufd) return;
                _ufd = value;
                NotifyPropertyChanged("UFD");
            }
        }
        public string Placa
        {
            get { return _placa; }
            set
            {
                if (value == _placa) return;
                _placa = value;
                NotifyPropertyChanged("Placa");
            }
        }
        public string NumNF
        {
            get { return _numNf; }
            set
            {
                if (value == _numNf) return;
                _numNf = value;
                NotifyPropertyChanged("NumNF");
            }
        }
        public string CodUF
        {
            get { return _codUF; }
            set
            {
                if (value == _codUF) return;
                _codUF = value;
                NotifyPropertyChanged("CodUF");
            }
        }
        public string Natureza
        {
            get { return _natureza; }
            set
            {
                if (value == _natureza) return;
                _natureza = value;
                NotifyPropertyChanged("Natureza");
            }
        }
        /*public string Chave
        {
            get { return _chave; }
            set
            {
                if (value == _chave) return;
                _chave = value;
                NotifyPropertyChanged("Chave");
            }
        }*/
        public ChavesNfe Chaves
        {
            get { return _chaves; }
            set
            {
                if (value == _chaves) return;
                _chaves = value;
                NotifyPropertyChanged("Chaves");
            }
        }
        public decimal Peso
        {
            get { return _peso; }
            set
            {
                if (value == _peso) return;
                _peso = value;
                NotifyPropertyChanged("Peso");
            }
        }
        public decimal ValorNF
        {
            get { return _valor; }
            set
            {
                if (value == _valor) return;
                _valor = value;
                NotifyPropertyChanged("Valor");
            }
        }
        public decimal ValBoleto
        {
            get { return _valBoleto; }
            set
            {
                if (value == _valBoleto) return;
                _valBoleto = value;
                NotifyPropertyChanged("ValBoleto");
            }
        }
        public decimal ValICMSP
        {
            get { return _valICMSP; }
            set
            {
                if (value == _valICMSP) return;
                _valICMSP = value;
                NotifyPropertyChanged("ValICMSP");
            }
        }
        public decimal ValICMSST
        {
            get { return _valICMSST; }
            set
            {
                if (value == _valICMSST) return;
                _valICMSST = value;
                NotifyPropertyChanged("ValICMSST");
            }
        }
        public DateTime Emissao
        {
            get { return _emissao; }
            set
            {
                if (value == _emissao) return;
                _emissao = value;
                NotifyPropertyChanged("Emissao");
            }
        }
        public string TipoEmissao
        {
            get { return _tipoEmissao; }
            set
            {
                if (value == _tipoEmissao) return;
                _tipoEmissao = value;
                NotifyPropertyChanged("TipoEmissao");
            }
        }
        public Veiculo Veiculo
        {
            get { return _veiculo; }
            set
            {
                if (value == _veiculo) return;
                _veiculo = value;
                NotifyPropertyChanged("Veiculo");
            }
        }
        public string pathNF
        {
            get { return _pathNf; }
            set
            {
                if (value == _pathNf) return;
                _pathNf = value;
                NotifyPropertyChanged("pathNF");
            }
        }
        public bool IsDelete
        {
            get { return _isdelete; }
            set
            {
                if (value == _isdelete) return;
                _isdelete = value;
                NotifyPropertyChanged("IsDelete");
            }
        }


        public void BeginEdit()
        {
            if (m_Editing != false) return;
            temp_NFMDFe = this.MemberwiseClone() as NFMDFe;
            m_Editing = true;
        }
        public void EndEdit()
        {
            if (m_Editing != true) return;
            temp_NFMDFe = null;
            m_Editing = false;
        }
        public void CancelEdit()
        {
            throw new NotImplementedException();
        }
    }
}
