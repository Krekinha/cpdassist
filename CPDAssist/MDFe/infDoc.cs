using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace CPDAssist.MDFe
{
    public class infDoc
    {
        public infMunDescarga infMunDescarga = new infMunDescarga();
    }

    public class infMunDescarga
    {
        [XmlElement(Order = 0)]
        public string cMunDescarga { get; set; }

        [XmlElement(Order = 1)]
        public string xMunDescarga { get; set; }

        [XmlElement("infNFe", Order = 2)]
        //[XmlArrayItem("chave")]
        public List<infNFe> Chave = new List<infNFe>();
    }

    public class infNFe 
    {
        [XmlElement]
        public string chNFe { get; set; }
    }
}
