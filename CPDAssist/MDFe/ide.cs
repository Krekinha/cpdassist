using System;
using System.Xml.Serialization;

namespace CPDAssist.MDFe
{
    public class ide
    {
        [XmlElement(Order = 0)]
        public int cUF { get; set; }

        [XmlElement(Order = 1)]
        public int tpAmb { get; set; }

        [XmlElement(Order = 2)]
        public int tpEmit { get; set; }

        [XmlElement(Order = 3)]
        public int mod { get; set; }

        [XmlElement(Order = 4)]
        public string serie { get; set; }

        [XmlElement(Order = 5)]
        public int nMDF { get; set; }

        [XmlElement(Order = 6)]
        public string cMDF { get; set; }

        [XmlElement(Order = 7)]
        public int cDV { get; set; }

        [XmlElement(Order = 8)]
        public int modal { get; set; }

        [XmlElement(Order = 9)]
        public string dhEmi { get; set; }

        [XmlElement(Order = 10)]
        public int tpEmis { get; set; }

        [XmlElement(Order = 11)]
        public int procEmi { get; set; }

        [XmlElement(Order = 12)]
        public string verProc { get; set; }

        [XmlElement(Order = 13)]
        public string UFIni { get; set; }

        [XmlElement(Order = 14)]
        public string UFFim { get; set; }

        [XmlElement(Order = 15)]
        public infMunCarrega infMunCarrega = new infMunCarrega();
    }

    public class infMunCarrega
    {
        [XmlElement(Order = 0)]
        public string cMunCarrega { get; set; }

        [XmlElement(Order = 1)]
        public string xMunCarrega { get; set; }
    }
}
