using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MSSPVirusScanner
{
    public static class MSSPLogger
    {
        public static void WriteToLog(string strLogPath, string strLogEntry)
        {
            try
            {
                if (!Directory.Exists("C:\\Work\\ScanLogs"))
                    Directory.CreateDirectory("C:\\Work\\ScanLogs");

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
