using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSSPVirusScanner.MSSPScanStrategies
{
    /// <summary>
    /// Strategy context class for signature analysis - the application communicates
    /// with the encapsulated algorithms through this class.
    /// </summary>
    public class MSSPSignatureAnaylsisStrategyContext
    {
        private MSSPSignatureAnalysisStrategy _msspStrategy;

        /// <summary>
        /// Class constructor, initialise the concrete strategy to communicate with
        /// </summary>
        /// <param name="msspStrategy"></param>
        public MSSPSignatureAnaylsisStrategyContext(MSSPSignatureAnalysisStrategy msspStrategy)
        {
            _msspStrategy = msspStrategy;
        }

        /// <summary>
        /// Invoke the Analyse Signature method on the concrete strategy
        /// </summary>
        /// <param name="strSignature">The signature of the virus</param>
        /// <param name="strHex">The hex value of the program we're scanning</param>
        /// <returns>A boolean value indicating if the hex contains the signaure</returns>
        public bool AnalyseSignature(string strSignature, string strHex)
        {
            return _msspStrategy.AnalyseSignature(strSignature, strHex);
        }
    }
}
