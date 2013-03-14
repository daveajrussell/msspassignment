using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSPVirusScanner.MSSPScanStrategies
{
    /// <summary>
    /// Concrete IndexOf strategy. 
    /// This string scanning method proved to be the fastest.
    /// Research shows that the IndexOf method has hand optimised assembly, 
    /// thus making it faster than any managed code implementation of a string scanner
    /// that I attempted. (http://www.blackbeltcoder.com/Articles/algorithms/fast-text-search-with-boyer-moore)
    /// </summary>
    public class IndexOfStrategy : MSSPSignatureAnalysisStrategy
    {
        /// <summary>
        /// Simply call the String.IndexOf method, to scan the hex value for a signature
        /// </summary>
        /// <param name="strSignature">The signature to search for</param>
        /// <param name="strHex">The hex value of the file to scan</param>
        /// <returns>A boolean value indicating if the signature was found</returns>
        public override bool AnalyseSignature(string strSignature, string strHex)
        {
            return strHex.IndexOf(strSignature, StringComparison.Ordinal) > 0 ? true : false;
        }
    }
}
