using System.Xml.Serialization;

namespace CPDAssist.MDFe
{
    public class seg
    {
        [XmlElement(Order = 0)]
        public infResp infResp = new infResp();

        [XmlElement(Order = 1)]
        public infSeg infSeg = new infSeg();

        [XmlElement(Order = 2)]
        public string nApol { get; set; }

        [XmlElement(Order = 3)]
        public string nAver { get; set; }
    }

    public class infResp
    {
        [XmlElement(Order = 0)]
        public int respSeg { get; set; }

        [XmlElement(Order = 1)]
        public string CNPJ { get; set; }
    }

    public class infSeg
    {
        [XmlElement(Order = 0)]
        public string xSeg { get; set; }

        [XmlElement(Order = 1)]
        public string CNPJ { get; set; }
    }
}
