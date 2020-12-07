using System.Xml.Serialization;

namespace CPDAssist.MDFe
{
    public class emit
    {
        [XmlElement(Order = 0)]
        public string CNPJ { get; set; }

        [XmlElement(Order = 1)]
        public string IE { get; set; }

        [XmlElement(Order = 2)]
        public string xNome { get; set; }

        [XmlElement(Order = 3)]
        public string xFant { get; set; }

        [XmlElement(Order = 4)]
        public enderEmit enderEmit = new enderEmit();
    }

    public class enderEmit
    {
        [XmlElement(Order = 0)]
        public string xLgr { get; set; }

        [XmlElement(Order = 1)]
        public string nro { get; set; }

        [XmlElement(Order = 2)]
        public string xBairro { get; set; }

        [XmlElement(Order = 3)]
        public long cMun { get; set; }

        [XmlElement(Order = 4)]
        public string xMun { get; set; }

        [XmlElement(Order = 5)]
        public string CEP { get; set; }

        [XmlElement(Order = 6)]
        public string UF { get; set; }

        [XmlElement(Order = 7)]
        public string fone { get; set; }

        [XmlElement(Order = 8)]
        public string email { get; set; }
    }
}
