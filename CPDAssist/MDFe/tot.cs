using System.Xml.Serialization;

namespace CPDAssist.MDFe
{
    public class tot
    {
        [XmlElement(Order = 0)]
        public int qNFe { get; set; }

        [XmlElement(Order = 1)]
        public decimal vCarga { get; set; }

        [XmlElement(Order = 2)]
        public string cUnid { get; set; }

        [XmlElement(Order = 3)]
        public string qCarga { get; set; }
    }
}
