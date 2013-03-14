using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSPVirusScanner.MSSPScanStrategies
{
    /// <summary>
    /// This is another method that proved (in some cases) to be faster
    /// than the Boyer-Moore implementation, it can be postulated that 
    /// this also has hand optimised assembly code for string searching.
    /// </summary>
    public class ContainsAnalysisStrategy : MSSPSignatureAnalysisStrategy
    {
        /// <summary>
        /// Conrete implementation of the skeleton algorithm,
        /// Call the String.Contains method on the hex value.
        /// </summary>
        /// <param name="strSignature">The virus signature to search for</param>
        /// <param name="strHex">The hex value of the file we're scanning</param>
        /// <returns>Boolean value indicating if the hex contains the signature</returns>
        public override bool AnalyseSignature(string strSignature, string strHex)
        {
            return strHex.Contains(strSignature);
        }
    }
}
