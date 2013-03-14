using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MSSPVirusScanner.Utils
{
    /// <summary>
    /// A class to Log certain actions to a file
    /// </summary>
    public static class MSSPLogger
    {
        /// <summary>
        /// Function to write an entry to the log
        /// </summary>
        /// <param name="strLogPath">The path of the log file</param>
        /// <param name="strLogEntry">The entry to save to the log</param>
        public static void WriteToLog(string strLogPath, string strLogEntry)
        {
            try
            {
                using (StreamWriter stream = new StreamWriter(strLogPath, true))
                {
                    stream.WriteLine(DateTime.Now + ": " + strLogEntry);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
