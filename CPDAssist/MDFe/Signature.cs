using System.Collections.Generic;
using System.Xml.Serialization;

namespace CPDAssist.MDFe
{
    public class Signature
    {
        [XmlElement(Order = 0)]
        public SignedInfo SignedInfo = new SignedInfo();

        [XmlElement(Order = 1)]
        public SignatureValue SignatureValue = new SignatureValue();

        [XmlElement(Order = 2)]
        public KeyInfo KeyInfo = new KeyInfo();
    }
    
    public class SignedInfo
    {
        [XmlElement(Order = 0)]
        public CanonicalizationMethod CanonicalizationMethod = new CanonicalizationMethod();

        [XmlElement(Order = 1)]
        public SignatureMethod SignatureMethod = new SignatureMethod();

        [XmlElement(Order = 2)]
        public Reference Reference = new Reference();
    }

    public class CanonicalizationMethod
    {
        [XmlAttribute] public string Algorithm = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
    }

    public class SignatureMethod
    {
        [XmlAttribute]
        public string Algorithm = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
    }

    public class Reference
    {
        [XmlAttribute]
        public string URI { get; set; }

        [XmlArray(Order = 0)]
        public List<Transform> Transforms = new List<Transform>();

        [XmlElement(Order = 1)]
        public DigestMethod DigestMethod = new DigestMethod();

        [XmlArray(Order = 2)]
        public List<string> DigestValue = new List<string>();
    }

    public class Transform
    {
        [XmlAttribute]
        public string Algorithm { get; set; }
    }

    public class DigestMethod
    {
        [XmlAttribute]
        public string Algorithm = "http://www.w3.org/2000/09/xmldsig#sha1";
    }

    public class SignatureValue
    {
        
    }

    public class KeyInfo
    {
        public X509Data X509Data = new X509Data();
    }

    public class X509Data
    {
        [XmlElement]
        public X509Certificate X509Certificate = new X509Certificate();
    }

    public class X509Certificate
    {
        
    }
}
