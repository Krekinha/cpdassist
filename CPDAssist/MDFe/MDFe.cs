
using System.Xml.Serialization;

namespace CPDAssist.MDFe
{
    [XmlRoot(Namespace = "http://www.portalfiscal.inf.br/mdfe")]
    public class MDFe
    {
        [XmlElement]
        public infMDFe infMDFe = new infMDFe();

        [XmlElement(Namespace = "http://www.w3.org/2000/09/xmldsig#")]
        public Signature Signature = new Signature();
    }
}
