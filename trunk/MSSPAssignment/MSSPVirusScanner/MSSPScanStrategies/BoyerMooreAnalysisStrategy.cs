using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSPVirusScanner.MSSPScanStrategies
{
    /// <summary>
    /// Concrete implementation of the Boyer-Moore algorithm.
    /// Inspiration for the implementation was drawn from the three following articles:
    /// http://www.stoimen.com/blog/2012/04/17/computer-algorithms-boyer-moore-string-search-and-matching/
    /// http://www.blackbeltcoder.com/Articles/algorithms/fast-text-search-with-boyer-moore
    /// http://www.codeproject.com/Articles/12781/Boyer-Moore-and-related-exact-string-matching-algo
    /// This did prove to be fast at scanning hex for signatures, it proved to be faster than
    /// the String.Contains method, but the String.IndexOf method trumped it.
    /// </summary>
    public class BoyerMooreAnalysisStrategy : MSSPSignatureAnalysisStrategy
    {
        /// <summary>
        /// Implement the body for the skeleton algorithm
        /// </summary>
        /// <param name="strSignature">The signature to scan for</param>
        /// <param name="strHex">The hex value to scan against</param>
        /// <returns>Boolean value indicating if there were any matches made</returns>
        public override bool AnalyseSignature(string strSignature, string strHex)
        {
            return BoyerMoore(strSignature, strHex);
        }

        /// <summary>
        /// Boyer-Moore algorithm implementation
        /// </summary>
        /// <param name="strSignature">The signature to scan for</param>
        /// <param name="strHex">The hex value to scan against</param>
        /// <returns>Boolean value indicating if there were any matches made</returns>
        private bool BoyerMoore(string strSignature, string strHex)
        {
            // Create an array of bad characters - These are characters
            // that are skipped in case of a mismatch, this speeding up the searching
            // The array must be size 71, the greatest hex value we have
            // is 'F', which is 70 in decimal. Anything smaller would throw
            // an out of bounds exception when an F character was encountered.
            byte[] arrBadCharacters = new byte[71];

            for (int i = 0; i < strSignature.Length - 1; i++)
            {
                arrBadCharacters[strSignature[i]] = (byte)(strSignature.Length - i - 1);
            }

            // Represents the index at which to start scanning
            int j = 0;

            // Iterate across the hex value whilst there is still
            // room for the signature to fit
            while (j <= (strHex.Length - strSignature.Length))
            {
                int k = strSignature.Length - 1;

                // Check for a match at the current position
                while (k >= 0 && strSignature[k] == strHex[j + k])
                    k--;
                
                // If a match is found we immediately break out of the algorithm and return true
                if (k < 0)
                {
                    return true;
                }

                // Otherwise we move on to the next comparison
                j += Math.Max(arrBadCharacters[strHex[j + k]] - strSignature.Length + 1 + k, 1);
            }

            // If no match was found, return false
            return false;
        }
    }
}
