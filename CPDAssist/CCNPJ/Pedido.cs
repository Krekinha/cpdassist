using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPDAssist.CCNPJ
{
    public class Pedido
    {
        public int Regiao { get; set; }
        public int Numero { get; set; }
        public string Vendedor { get; set; }
        public decimal ValorTotal { get; set; }
    }
}
