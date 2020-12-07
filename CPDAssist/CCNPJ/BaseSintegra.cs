using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPDAssist.CCNPJ
{
    public class BaseSintegra
    {
        public string CNPJ { get; set; }
        public string IE { get; set; }
        public string Razao { get; set; }
        public string SituacaoCNPJ { get; set; }

        public BaseSintegra(string cnpj, string ie, string razao, string situacaoCnpj)
        {
            CNPJ = cnpj;
            IE = ie;
            Razao = razao;
            SituacaoCNPJ = situacaoCnpj;
        }
    }
}
