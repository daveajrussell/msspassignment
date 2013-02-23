using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSSPVirusSignatureDatabase.Models
{
    [Serializable()]
    public class XMLVirusSignature
    {
        [XmlElement("SIGNATURE_ID")]
        public int SIGNATURE_ID { get; set; }

        [XmlElement("SIGNATURE_STRING")]
        public string SIGNATURE_STRING { get; set; }
    }

    [Serializable()]
    [XmlRoot("VIRUS_SIGNATURES")]
    public class XMLVirusSignatures
    {
        [XmlArray("SIGNATURES")]
        [XmlArrayItem("SIGNATURE", typeof(XMLVirusSignature))]
        public XMLVirusSignature[] Signatures { get; set; }
    }
}
