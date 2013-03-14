using MSSPVirusScanner.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSPVirusScanner.MSSPScanStrategies
{
    /// <summary>
    /// Full scan concrete strategy class.
    /// Quick scan scans all of the defined files for virus signatures
    /// </summary>
    public class FullScanStrategy : MSSPFileSignatureAnalysisStrategy
    {
        private string[] _fileExtensionTypes;

        /// <summary>
        /// Class constructor, initialises an array of file extension types to scan for
        /// </summary>
        public FullScanStrategy()
        {
            _fileExtensionTypes = new string[]
            {
                FileExtensionTypes.AAC,
                FileExtensionTypes.EML,
                FileExtensionTypes.HLP,
                FileExtensionTypes.LIB,
                FileExtensionTypes.LNK,
                FileExtensionTypes.MPEG,
                FileExtensionTypes.MSACCESS,
                FileExtensionTypes.MSC,
                FileExtensionTypes.MSCOMPILEDHTMLHELP,
                FileExtensionTypes.MSDOS,
                FileExtensionTypes.MSOFFICEAPP,
                FileExtensionTypes.OBJ,
                FileExtensionTypes.OOXML,
                FileExtensionTypes.PGM,
                FileExtensionTypes.PRC,
                FileExtensionTypes.REG,
                FileExtensionTypes.RTF,
                FileExtensionTypes.SYS
            };
        }

        /// <summary>
        /// Concrete implementation of the skeleton algorithm.
        /// Search through all of the file extensions.
        /// </summary>
        /// <param name="strHex">The hex in which to search for file extensions</param>
        /// <returns>Boolean value indicating if the hex contains the file signature</returns>
        public override bool AnalyseFileSignature(string strHex)
        {
            foreach (var ext in _fileExtensionTypes)
                if (strHex.StartsWith(ext))
                    return true;
            return false;
        }
    }
}
