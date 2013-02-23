using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MSSPVirusSignatureDatabase.Models
{
    public class VIRUS_SIGNATURE
    {
        public int SIGNATURE_ID { get; set; }
        public string SIGNATURE_STRING { get; set; }
    }
}
