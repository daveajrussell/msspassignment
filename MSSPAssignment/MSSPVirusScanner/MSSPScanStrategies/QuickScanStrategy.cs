using MSSPVirusScanner.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSPVirusScanner.MSSPScanStrategies
{
    /// <summary>
    /// Quick Scan concrete strategy class.
    /// For quick scan we only concern ourselves with MS DOS executable files
    /// </summary>
    public class QuickScanStrategy : MSSPFileSignatureAnalysisStrategy
    {
        /// <summary>
        /// Determine if a hex value contains the MS DOS file signature (MZ)
        /// </summary>
        /// <param name="strHex">The hex value to search</param>
        /// <returns>A boolean value indicating if the file is an MS DOS executable</returns>
        public override bool AnalyseFileSignature(string strHex)
        {
            return strHex.StartsWith(FileExtensionTypes.MSDOS);
        }
    }
}
