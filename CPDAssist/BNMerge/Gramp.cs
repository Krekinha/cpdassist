using CPDAssit.BNMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPDAssit.BNMerge
{
    public class Gramp
    {
        public dadosBT Boleto { get; set; }
        public List<dadosNF> NFcBol = new List<dadosNF>();
        public int Rom { get; set; }
        public int[] Pages { get; set; }
    }
}
