using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Timers;
using System.Diagnostics;

namespace MSSPAssignment
{
    public class MSSPProgram
    {
        private const string INITIAL_DIRECTORY = "C:\\Program Files (x86)";
        private static string LOG_PATH;
        private static int intFileCount = 0;
        private static int intDirCount = 0;
        private static Stopwatch mainTimer;

        public static void Main(string[] args)
        {
            LOG_PATH = string.Format("C:\\Work\\ScanLog_{0}_{1}.txt", DateTime.Now.Ticks, DateTime.Now.ToString("yyyyMMdd"));
            
            mainTimer = new Stopwatch();
            mainTimer.Start();
            RecurseDirectory(INITIAL_DIRECTORY);
            mainTimer.Stop();

            Console.Clear();
            Console.WriteLine("Scan of " + INITIAL_DIRECTORY + " completed.");
            WriteToLog("Scan of " + INITIAL_DIRECTORY + " completed.");
            Console.WriteLine(intDirCount + " directories and " + intFileCount + " files scanned in " + mainTimer.Elapsed);
            WriteToLog(intDirCount + " directories and " + intFileCount + " files scanned in " + mainTimer.Elapsed);
            Console.ReadLine();
        }

        public static void RecurseDirectory(string strDirectory)
        {
            string[] dirs = Directory.GetDirectories(strDirectory);

            if (dirs.Length == 0)
            {
                Scan(strDirectory);
            }
            else
            {
                foreach (var dir in dirs)
                {
                    RecurseDirectory(dir);
                    intDirCount++;
                }
            }
        }

        public static void Scan(string strDirectory)
        {
            Stopwatch dirTimer = new Stopwatch();
            dirTimer.Start();

            string[] files = Directory.GetFiles(strDirectory);

            if (0 == files.Length)
            {
                WriteToLog("No executables found in " + strDirectory);
            }
            else
            {
                foreach (var file in files)
                {
                    Console.Clear();
                    Console.WriteLine("Scanning Directory: " + strDirectory + "\n");
                    WriteToLog("Scanning Directory: " + strDirectory);

                    Console.WriteLine("Scanning File: " + file);
                    WriteToLog("Scanning File: " + file);

                    Console.WriteLine("Elapsed Time: " + mainTimer.ElapsedMilliseconds + "\n");
                    WriteToLog("Elapsed Time: " + mainTimer.ElapsedMilliseconds);

                    /*
                    var md5HashString = GetComputedMD5Hash(file);

                    if (null != md5HashString)
                        WriteToLog("MD5: " + md5HashString);

                    var crc32HashString = GetComputedCRC32Hash(file);

                    if (null != crc32HashString)
                        WriteToLog("CRC32: " + crc32HashString);
                    */

                    string hex = GetHexString(file);

                    if (null != hex)
                    {
                        if (hex.StartsWith(FileExtensionTypes.EXE))
                        {
                            WriteToLog("EXE!");
                        }
                        else if (hex.StartsWith(FileExtensionTypes.MSI))
                        {
                            WriteToLog("MSI!");
                        }
                    }

                    /*if(null != hex)
                        WriteToLog("Hex: " + hex);*/

                    intFileCount++;
                }
                dirTimer.Stop();
                WriteToLog("Scan of " + strDirectory);
            }
        }

        /*
        public static string GetComputedMD5Hash(string strFile)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    using (FileStream oFileStream = new FileStream(strFile, FileMode.Open))
                    {
                        return BitConverter.ToString(md5.ComputeHash(oFileStream), 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error scanning " + strFile + ". " + ex.Message);
                WriteToLog("Error scanning " + strFile + ". " + ex.Message);
                return null;
            }
        }

        public static string GetComputedCRC32Hash(string strFile)
        {
            try
            {
                using (var crc32 = CRC32.Create())
                {
                    using (FileStream oFileStream = new FileStream(strFile, FileMode.Open))
                    {
                        return BitConverter.ToString(crc32.ComputeHash(oFileStream), 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error scanning " + strFile + ". " + ex.Message);
                WriteToLog("Error scanning " + strFile + ". " + ex.Message);
                return null;
            }
        }
        */

        public static string GetHexString(string strFile)
        {
            try
            {
                using (FileStream oFileStream = new FileStream(strFile, FileMode.Open))
                {
                    byte[] arrBytes = new byte[oFileStream.Length];

                    oFileStream.Read(arrBytes, 0, int.Parse(oFileStream.Length.ToString()));
                    return BitConverter.ToString(arrBytes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error scanning " + strFile + ". " + ex.Message);
                WriteToLog("Error scanning " + strFile + ". " + ex.Message);
                return null;
            }
        }

        private static void WriteToLog(string strLogEntry)
        {
            using (StreamWriter stream = new StreamWriter(LOG_PATH, true))
            {
                stream.WriteLine(strLogEntry);
            }
        }
    }
}
