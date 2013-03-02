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

        public Signatures GetKnownSignatures()
        {
            try
            {
                var oDoc = new XDocument(new XElement("Signatures", from signature in dbContext.VIRUS_SIGNATURE.AsEnumerable()
                                                                    select new XElement("Signature",
                                                                        new XAttribute("ID", signature.SIGNATURE_ID),
                                                                        new XAttribute("Location", signature.SIGNATURE_LOCATION),
                                                                        new XAttribute("String", signature.SIGNATURE_STRING),
                                                                        new XAttribute("Name", signature.SIGNATURE_NAME))));

                File.Delete("MSSPVirusSignatures.xml");

                using (var fs = File.OpenWrite("MSSPVirusSignatures.xml"))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(oDoc.ToString());
                    }
                }

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Signatures));
                using (var xmlStreamReader = XmlReader.Create("MSSPVirusSignatures.xml"))
                {
                    return (Signatures)xmlSerializer.Deserialize(xmlStreamReader);
                }
            }
            catch (Exception e)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Signatures));
                using (var xmlStreamReader = XmlReader.Create("MSSPVirusSignatures.xml"))
                {
                    return (Signatures)xmlSerializer.Deserialize(xmlStreamReader);
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
