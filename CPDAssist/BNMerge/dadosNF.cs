using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPDAssit.BNMerge
{
    public class dadosNF
    {
        public int Num { get; set; }
        public int Folha { get; set; }
        public int TFolha { get; set; }
        public int Romaneio { get; set; }
        public int Page { get; set; }
        public string Boleto { get; set; }
        public string FPagamento { get; set; }

        public dadosNF(int num, int folha, int tfolha, int romaneio, int page, string boleto)
        {
            this.Num = num;
            this.Folha = folha;
            this.TFolha = tfolha;
            this.Romaneio = romaneio;
            this.Page = page;
            this.Boleto = boleto;
        }

        public dadosNF()
        { }
    }

}
