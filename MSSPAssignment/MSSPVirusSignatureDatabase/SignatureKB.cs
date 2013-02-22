using MSSPVirusSignatureDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSPVirusSignatureDatabase
{
    public class SignatureKB
    {
        private VirusSignatureContext dbContext;

        public SignatureKB()
        {
            dbContext = new VirusSignatureContext();
        }

        public IEnumerable<VIRUS_SIGNATURE> GetKnownSignatures()
        {
            return from signature in dbContext.VIRUS_SIGNATURE
                       select signature;
        }

        public void Close()
        {
            if (null != dbContext)
            {
                dbContext.Dispose();
            }
        }
    }
}
