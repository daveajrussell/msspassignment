using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSPVirusScanner.MSSPScanStrategies
{
    /// <summary>
    /// Strategy context class for file extension analysis - the application communicates
    /// with the encapsulated algorithms through this class.
    /// </summary>
    public class MSSPFileSignatureAnalysisStrategyContext
    {
        private MSSPFileSignatureAnalysisStrategy _strategy;

        /// <summary>
        /// Class constructor, initialise the concrete strategy to communicate with
        /// </summary>
        /// <param name="strategy"></param>
        public MSSPFileSignatureAnalysisStrategyContext(MSSPFileSignatureAnalysisStrategy strategy)
        {
            _strategy = strategy;
        }

        /// <summary>
        /// Invoke the file signature analysis method on the concrete strategy
        /// </summary>
        /// <param name="strHex">The hex value of the file</param>
        /// <returns>Boolean value indicating if the file is of a type we're scanning for</returns>
        public bool AnalyseFileSignature(string strHex)
        {
            return _strategy.AnalyseFileSignature(strHex);
        }
    }
}
