using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSPVirusScanner.MSSPScanStrategies
{
    /// <summary>
    /// Skeleton algoirthm for analysing a hex value for a given file signature
    /// </summary>
    public abstract class MSSPFileSignatureAnalysisStrategy
    {
        /// <summary>
        /// The method that must be implemented by any inheriting concrete strategy classes
        /// </summary>
        /// <param name="strHex">The hex to scan for file signatures</param>
        /// <returns>A boolean value indicating if the hex contains a signature</returns>
        public abstract bool AnalyseFileSignature(string strHex);
    }
}
