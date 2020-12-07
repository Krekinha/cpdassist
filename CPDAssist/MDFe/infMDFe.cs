using System.Xml.Serialization;

namespace CPDAssist.MDFe
{
    public class infMDFe
    {
        [XmlAttribute(AttributeName = "Id")]
        public string id { get; set; }
        [XmlAttribute(AttributeName = "versao")]
        public string versao { get; set; }
        public ide ide = new ide();
        public emit emit = new emit();
        public infModal infModal = new infModal();
        public infDoc infDoc = new infDoc();
        public seg seg = new seg();
        public tot tot = new tot();
        public infAdic infAdic = new infAdic();
    }
}
