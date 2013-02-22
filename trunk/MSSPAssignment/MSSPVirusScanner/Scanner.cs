using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MSSPVirusSignatureDatabase.Models;
using MSSPVirusSignatureDatabase;
using System.Threading;
using System.ComponentModel;

namespace MSSPVirusScanner
{
    public class Scanner
    {
        private string LogPath { get; set; }
        private BackgroundWorker Worker { get; set; }
        private DoWorkEventArgs WorkerEvent { get; set; }
        private MSSPVirusScannerForm Context{ get; set; }
        private SignatureKB Signatures;
        private IEnumerable<VIRUS_SIGNATURE> VirusSignatures { get; set; }
        
        private static int intTotalFileCount = 0;
        private static int intDirectoryFileCount = 0;
        private static int intDirCount = 0;
        private static Stopwatch mainTimer;

        public delegate void ProgressUpdateDelegate(string strDirectory, string strFile, string strFileCount);
        public delegate void ScanCompleteDelegate(string strDirectory, string strDirectoryCount, string strFileCount, long lngElapsedMillis);
        public delegate void VirusDetectedDelegate(string strDirectory, string strFile);

        public event ProgressUpdateDelegate ProgressUpdateHandler;
        public event ScanCompleteDelegate ScanCompleteHandler;
        public event VirusDetectedDelegate VirusDetectedHandler;

        public Scanner(string strLogPath, MSSPVirusScannerForm oMSSPVirusScannerForm)
        {
            LogPath = strLogPath;
            Context = oMSSPVirusScannerForm;
            Signatures = new SignatureKB();
            VirusSignatures = Signatures.GetKnownSignatures();

            intTotalFileCount = 0;
            intDirectoryFileCount = 0;
            intDirCount = 0;
        }

        public void InitiateScan(BackgroundWorker oWorker, DoWorkEventArgs e)
        {
            Worker = oWorker;
            WorkerEvent = e;

            mainTimer = new Stopwatch();
            mainTimer.Start();
            RecurseDirectory(e.Argument.ToString());
            mainTimer.Stop();

            Logger.WriteToLog(LogPath, "Scan of " + e.Argument.ToString() + " Completed.");
            Logger.WriteToLog(LogPath, intDirCount + " Directories and " + intTotalFileCount + " Files Scanned in " + mainTimer.Elapsed);

            if (null != ScanCompleteHandler)
                Context.Invoke(ScanCompleteHandler, e.Argument.ToString(), intDirCount.ToString(), intTotalFileCount.ToString(), mainTimer.ElapsedMilliseconds);

            Signatures.Close();
        }

        private void RecurseDirectory(string strDirectory)
        {
            if (Worker.CancellationPending)
            {
                Signatures.Close();
            }
            else
            {
                try
                {
                    string[] dirs = Directory.GetDirectories(strDirectory);

                    if (0 == dirs.Length)
                    {
                        Worker.ReportProgress(0);
                        Scan(strDirectory);
                    }
                    else
                        foreach (var dir in dirs)
                        {
                            RecurseDirectory(dir);
                            intDirCount++;
                        }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog(LogPath, "Error scanning " + strDirectory + ". " + ex.Message);

                    if (ex is ThreadAbortException || ex is OutOfMemoryException)
                        Signatures.Close();
                }
            }
        }

        private void Scan(string strDirectory)
        {
            string[] files = Directory.GetFiles(strDirectory);
            intDirectoryFileCount = 0;

            if (0 != files.Length)
                foreach (var strFile in files)
                {
                    if (null != ProgressUpdateHandler)
                        Context.Invoke(ProgressUpdateHandler, strDirectory, strFile, intTotalFileCount.ToString());

                    int intPercentComplete = (int)((float)intDirectoryFileCount / (float)files.Length * 100);

                    Worker.ReportProgress(intPercentComplete);

                    ExamineFileExtensionSignature(strDirectory, strFile);
                    intTotalFileCount++;
                    intDirectoryFileCount++;
                }
        }

        private void ExamineFileExtensionSignature(string strDirectory, string strFile)
        {
            string strHex = null;
            byte[] arrBytes = new byte[2];

            try
            {
                using (FileStream oFileStream = new FileStream(strFile, FileMode.Open))
                {
                    oFileStream.Read(arrBytes, 0, 2);
                    strHex = BitConverter.ToString(arrBytes);
                }

                if (null != strHex)
                    if (strHex.StartsWith(FileExtensionTypes.DOS))
                        ScanHex(strDirectory, strFile);
            }
            catch (Exception ex)
            {
                Logger.WriteToLog(LogPath, "Error Examining " + strFile.Substring(strFile.LastIndexOf('\\') + 1) + " Extension Signature. Error: " + ex.Message);
            }
        }

        private void ScanHex(string strDirectory, string strFile)
        {
            string strHex = null;
            try
            {
                using (FileStream oFileStream = new FileStream(strFile, FileMode.Open))
                {
                    Logger.WriteToLog(LogPath, "Examining: " + strFile.Substring(strFile.LastIndexOf('\\') + 1));

                    byte[] arrBytes = new byte[oFileStream.Length];
                    oFileStream.Read(arrBytes, 0, int.Parse(oFileStream.Length.ToString()));
                    strHex = BitConverter.ToString(arrBytes);

                    foreach (var signature in VirusSignatures)
                    {
                        if (strHex.Contains(signature.SIGNATURE_STRING))
                        {
                            Logger.WriteToLog(LogPath, "Virus with ID:" + signature.SIGNATURE_ID + " And Signature:" + signature.SIGNATURE_STRING + " Detected in File:" + strFile.Substring(strFile.LastIndexOf('\\') + 1));

                            if (null != VirusDetectedHandler)
                                Context.Invoke(VirusDetectedHandler, strFile.Substring(strFile.LastIndexOf('\\') + 1), strDirectory);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteToLog(LogPath, "Error Examining " + strFile.Substring(strFile.LastIndexOf('\\') + 1) + " for Virus Signatures. Error: " + ex.Message);
            }
        }
    }
}
