using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace CPDAssist.MDFe
{
    public class infModal
    {
        [XmlAttribute(AttributeName = "versaoModal")]
        public string versaoModal { get; set; }

        public rodo rodo = new rodo();
             
    }

    public class rodo
    {
        //public infANTT infANTT = new infANTT() { RNTRC = "00000000" };
        public infANTT infANTT = new infANTT();
        public veicTracao veicTracao = new veicTracao();
        public string codAgPorto = "0000000000000000";
    }

    public class infANTT
    {
        [XmlElement]
        public string RNTRC { get; set; }
    }

    public class veicTracao
    {
        [XmlElement(Order = 0)]
        public string cInt { get; set; }

        [XmlElement(Order = 1)]
        public string placa { get; set; }

        [XmlElement(Order = 2)]
        public int tara { get; set; }

        [XmlElement(Order = 3)]
        public int capKG { get; set; }

        [XmlElement(Order = 4)]
        public prop prop = new prop();

        [XmlElement(Order = 5)]
        public condutor condutor = new condutor();

        [XmlElement(Order = 6)]
        public string tpRod { get; set; }

        [XmlElement(Order = 7)]
        public string tpCar { get; set; }

        [XmlElement(Order = 8)]
        public string UF { get; set; }
    }

    public class condutor
    {
        [XmlElement(Order = 0)]
        public string xNome { get; set; }

        [XmlElement(Order = 1)]
        public string CPF { get; set; }
    }

    public class prop
    {
        [XmlElement(Order = 0)]
        public string CPF { get; set; }

        [XmlElement(Order = 1)]
        public string RNTRC { get; set; }

        [XmlElement(Order = 2)]
        public string xNome { get; set; }

        [XmlElement(Order = 3)]
        public string IE { get; set; }

        [XmlElement(Order = 4)]
        public string UF { get; set; }

        [XmlElement(Order = 5)]
        public string tpProp { get; set; }
    }
}
