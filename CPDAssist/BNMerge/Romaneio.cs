using CPDAssit.BNMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPDAssit.BNMerge
{
    public class Romaneio
    {
        public int Rom { get; set; }
        public List<Gramp> Grampeados = new List<Gramp>();
        public List<dadosNF> NFsBol = new List<dadosNF>();
        public string PageSeq { get; set; }
    }
}
