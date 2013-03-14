using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MSSPVirusSignatureDatabase.Models
{
    /// <summary>
    /// Entity Framework Virus model
    /// </summary>
    public class VIRUS_SIGNATURE
    {
        public int SIGNATURE_ID { get; set; }
        public string SIGNATURE_LOCATION { get; set; }
        public string SIGNATURE_STRING { get; set; }
        public string SIGNATURE_NAME { get; set; }

        public VIRUS_SIGNATURE()
        {

        }
    }
}
