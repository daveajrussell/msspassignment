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
        private static int intFileCount = 0;
        private static int intDirCount = 0;

        public static void Main(string[] args)
        {
            Stopwatch mainTimer = new Stopwatch();
            mainTimer.Start();
            RecurseDirectory(INITIAL_DIRECTORY);
            mainTimer.Stop();
            Console.WriteLine("Scan of " + INITIAL_DIRECTORY + " completed.");
            Console.WriteLine(intDirCount + " directories and " + intFileCount + " files scanned in " + mainTimer.ElapsedMilliseconds + " milliseconds.");
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

            string[] files = Directory.GetFiles(strDirectory, "*.exe");

            if (0 == files.Length)
            {
                Console.WriteLine("No executables found in " + strDirectory);
            }
            else
            {

                Console.WriteLine("Scanning " + strDirectory + "\n");

                foreach (var file in files)
                {
                    Console.WriteLine("Computing MD5 of " + file);
                    Stopwatch computeMD5HashTimer = new Stopwatch();

                    computeMD5HashTimer.Start();
                    var md5Hash = GetComputedMD5Hash(file);
                    computeMD5HashTimer.Stop();

                    if (null != md5Hash)
                        Console.WriteLine("MD5: " + BitConverter.ToString(md5Hash, 0) + " in " + computeMD5HashTimer.ElapsedMilliseconds + " milliseconds\n");


                    Console.WriteLine("Computing CRC32 of " + file);
                    Stopwatch computeCRC32HashTimer = new Stopwatch();

                    computeCRC32HashTimer.Start();
                    var crc32Hash = GetComputedCRC32Hash(file);
                    computeCRC32HashTimer.Stop();

                    if (null != crc32Hash)
                        Console.WriteLine("CRC32: " + BitConverter.ToString(crc32Hash, 0) + " in " + computeCRC32HashTimer.ElapsedMilliseconds + " milliseconds\n");

                    intFileCount++;
                }

                dirTimer.Stop();
                Console.WriteLine("Scan of " + strDirectory + " in " + dirTimer.ElapsedMilliseconds + " milliseconds\n");
            }
        }

        public static byte[] GetComputedMD5Hash(string strFile)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = File.OpenRead(strFile))
                    {
                        return md5.ComputeHash(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error scanning " + strFile + ". " + ex.Message);
                return null;
            }
        }

        public static byte[] GetComputedCRC32Hash(string strFile)
        {
            try
            {
                using (var crc32 = CRC32.Create())
                {
                    using (var stream = File.OpenRead(strFile))
                    {
                        return crc32.ComputeHash(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error scanning " + strFile + ". " + ex.Message);
                return null;
            }
        }

        private static string GetTimeStringFromMillis(long lngMillis)
        {
            TimeSpan tsTime = TimeSpan.FromMilliseconds(lngMillis);
            return string.Format("{0}:{1}:{2}", tsTime.TotalHours, tsTime.TotalMinutes, tsTime.TotalSeconds);
        }
    }
}
