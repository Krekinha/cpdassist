using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPDAssit.BNMerge
{
    public class dadosBT
    {
        public int[] RefNF { get; set; }
        public int Romaneio { get; set; }
        public int Page { get; set; }

        public dadosBT(int[] refNf, int romaneio, int page)
        {
            this.RefNF = refNf;
            this.Romaneio = romaneio;
            this.Page = page;
        }

        public dadosBT()
        { }
    }
}
