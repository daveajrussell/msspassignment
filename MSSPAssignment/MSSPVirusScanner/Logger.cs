using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MSSPVirusScanner
{
    public static class Logger
    {
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
