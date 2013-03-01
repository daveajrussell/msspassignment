using MSSPVirusSignatureDatabase.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MSSPVirusSignatureDatabase
{
    public class MSSPSignatureDatabase
    {
        private VirusSignatureContext dbContext;

        public MSSPSignatureDatabase()
        {
            dbContext = new VirusSignatureContext();
        }

        public XMLVirusSignatures GetKnownSignatures()
        {
            try
            {
                var data = (from signature in dbContext.VIRUS_SIGNATURE
                            select signature).ToList();

                var oDoc = new XDocument(new XElement("VIRUS_SIGNATURES", new XElement("SIGNATURES", from signature in data.AsEnumerable()
                                                                                                     select new XElement("SIGNATURE",
                                                                                                         new XAttribute("SIGNATURE_ID", signature.SIGNATURE_ID),
                                                                                                         new XAttribute("SIGNATURE_LOCATION", signature.SIGNATURE_LOCATION),
                                                                                                         new XAttribute("SINGATURE_STRING", signature.SIGNATURE_STRING)))));

                File.Delete("MSSPVirusSignatures.xml");

                using (var stream = new StreamWriter("MSSPVirusSignatures.xml", true))
                {
                    stream.Write(oDoc.ToString());
                }

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(XMLVirusSignatures));
                using (var xmlStreamReader = new StreamReader("MSSPVirusSignatures.xml"))
                {
                    return (XMLVirusSignatures)xmlSerializer.Deserialize(xmlStreamReader);
                }
            }
            catch (Exception e)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(XMLVirusSignatures));
                using (var xmlStreamReader = new StreamReader("MSSPVirusSignatures.xml"))
                {
                    return (XMLVirusSignatures)xmlSerializer.Deserialize(xmlStreamReader);
                }
            }
        }

        public void Close()
        {
            if (null != dbContext)
                dbContext.Dispose();
        }
    }
}
