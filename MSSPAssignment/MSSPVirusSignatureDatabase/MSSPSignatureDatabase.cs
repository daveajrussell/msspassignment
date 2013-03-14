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
    /// <summary>
    /// Entity Framework class used to retrieve a collection of 
    /// virus signatures from a database.
    /// </summary>
    public class MSSPSignatureDatabase
    {
        private VirusSignatureContext dbContext;

        /// <summary>
        /// Class constructor to initialise the database context
        /// variable - allowing us to communicate with the database
        /// </summary>
        public MSSPSignatureDatabase()
        {
            dbContext = new VirusSignatureContext();
        }

        /// <summary>
        /// Method to query the database for a collection of signatures, represented as XML.
        /// This xml is then saved to a file so the signatures can be saved in case the connection
        /// to the database is not available for the next scan.
        /// </summary>
        /// <returns></returns>
        public Signatures GetKnownSignatures()
        {
            try
            {
                // Get a collection of signatures and construct into XML
                var oDoc = new XDocument(new XElement("Signatures", from signature in dbContext.VIRUS_SIGNATURE.AsEnumerable()
                                                                    select new XElement("Signature",
                                                                        new XAttribute("ID", signature.SIGNATURE_ID),
                                                                        new XAttribute("Location", signature.SIGNATURE_LOCATION),
                                                                        new XAttribute("String", signature.SIGNATURE_STRING),
                                                                        new XAttribute("Name", signature.SIGNATURE_NAME))));

                // Delete the old file
                File.Delete("MSSPVirusSignatures.xml");

                // Create a new xml file
                using (var fs = File.OpenWrite("MSSPVirusSignatures.xml"))
                {
                    // Write the xml into the file stream for saving
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(oDoc.ToString());
                    }
                }

                // Using the auto generated Signatures class (using XSD.exe), deserialise 
                // the XML file into a collection of signatures, usable by the main app
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Signatures));
                using (var xmlStreamReader = XmlReader.Create("MSSPVirusSignatures.xml"))
                {
                    return (Signatures)xmlSerializer.Deserialize(xmlStreamReader);
                }
            }
            // If an error occurs, such as no internet connection, fallback to the previous file
            catch (Exception e)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Signatures));
                using (var xmlStreamReader = XmlReader.Create("MSSPVirusSignatures.xml"))
                {
                    return (Signatures)xmlSerializer.Deserialize(xmlStreamReader);
                }
            }
        }

        /// <summary>
        /// When we're finished, close the connection to the database
        /// </summary>
        public void Close()
        {
            if (null != dbContext)
                dbContext.Dispose();
        }
    }
}
