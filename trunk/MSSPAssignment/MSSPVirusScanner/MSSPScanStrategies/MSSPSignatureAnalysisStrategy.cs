using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSPVirusScanner.MSSPScanStrategies
{
    /// <summary>
    /// Abstract Strategy class that our Concrete strategy algorithms will override
    /// </summary>
    public abstract class MSSPSignatureAnalysisStrategy
    {
        /// <summary>
        /// Defines the skeleton of the algorithm, any classes inheriting
        /// the strategy must implement this method.
        /// </summary>
        /// <param name="strSignature">The signature of the virus</param>
        /// <param name="strHex">The hex value of the file being scanned</param>
        /// <returns></returns>
        public abstract bool AnalyseSignature(string strSignature, string strHex);
    }
}
