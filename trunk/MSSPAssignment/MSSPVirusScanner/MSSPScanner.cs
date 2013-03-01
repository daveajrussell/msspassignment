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
    public class MSSPScanner
    {
        private string LogPath { get; set; }
        private BackgroundWorker Worker { get; set; }
        private DoWorkEventArgs WorkerEvent { get; set; }
        private MSSPVirusScannerForm Context { get; set; }
        private MSSPSignatureDatabase Signatures;
        private XMLVirusSignatures VirusSignatures { get; set; }

        private static int intAccumulatingFileCount;
        private static int intTotalFileCount;
        private static int intDirectoryCount;
        private static Stopwatch mainTimer;

        public delegate void ProgressUpdateDelegate(string strDirectory, string strFile, string strFileCount);
        public delegate void ScanCompleteDelegate(string strDirectory, string strDirectoryCount, string strFileCount, long lngElapsedMillis);
        public delegate void VirusDetectedDelegate(string strDirectory, string strFile);

        public event ProgressUpdateDelegate ProgressUpdateHandler;
        public event ScanCompleteDelegate ScanCompleteHandler;
        public event VirusDetectedDelegate VirusDetectedHandler;

        public MSSPScanner(string strLogPath, MSSPVirusScannerForm oMSSPVirusScannerForm)
        {
            intAccumulatingFileCount = 1;
            intTotalFileCount = 1;
            intDirectoryCount = 1;

            LogPath = strLogPath;
            Context = oMSSPVirusScannerForm;
            Signatures = new MSSPSignatureDatabase();
            VirusSignatures = Signatures.GetKnownSignatures();
        }

        public void InitiateScan(BackgroundWorker oWorker, DoWorkEventArgs e)
        {
            Worker = oWorker;
            WorkerEvent = e;

            mainTimer = new Stopwatch();
            mainTimer.Start();
            RecurseDirectory(e.Argument.ToString());
            mainTimer.Stop();

            MSSPLogger.WriteToLog(LogPath, "Scan of " + e.Argument.ToString() + " Completed.");
            MSSPLogger.WriteToLog(LogPath, intDirectoryCount + " Directories and " + intTotalFileCount + " Files Scanned in " + mainTimer.Elapsed);

            if (null != ScanCompleteHandler)
            {
                Context.Invoke(ScanCompleteHandler, e.Argument.ToString(), intDirectoryCount.ToString(), intTotalFileCount.ToString(), mainTimer.ElapsedMilliseconds);
            }

            Signatures.Close();
        }

        private void RecursiveFileCounter(string strDirectory)
        {
            try
            {
                foreach (var dir in Directory.GetDirectories(strDirectory))
                {
                    int count = Directory.GetFiles(strDirectory).Length;
                    intAccumulatingFileCount += count;
                    RecurseDirectory(dir);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void RecurseDirectory(string strDirectory)
        {
            if (Worker.CancellationPending)
                Signatures.Close();
            else
            {
                try
                {
                    string[] dirs = Directory.GetDirectories(strDirectory);

                    if (0 == dirs.Length)
                    {
                        Scan(strDirectory);
                    }
                    else
                        foreach (var dir in dirs)
                        {
                            RecurseDirectory(dir);
                            intDirectoryCount++;
                        }
                }
                catch (Exception ex)
                {
                    MSSPLogger.WriteToLog(LogPath, "Error scanning " + strDirectory + ". " + ex.Message);

                    if (ex is ThreadAbortException || ex is OutOfMemoryException)
                        Signatures.Close();
                }
            }
        }

        private void Scan(string strDirectory)
        {
            string[] files = Directory.GetFiles(strDirectory);

            if (0 != files.Length)
            {
                foreach (var strFile in files)
                {
                    if (null != ProgressUpdateHandler)
                        Context.Invoke(ProgressUpdateHandler, strDirectory, strFile, intTotalFileCount.ToString());

                    int intPercentComplete = (int)((float)intAccumulatingFileCount / (float)files.Length * 100);

                    Worker.ReportProgress(intPercentComplete);

                    ExamineFileExtensionSignature(strDirectory, strFile);
                    intAccumulatingFileCount++;
                    intTotalFileCount++;
                }
            }
            intAccumulatingFileCount = 0;

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
                MSSPLogger.WriteToLog(LogPath, "Error Examining " + strFile.Substring(strFile.LastIndexOf('\\') + 1) + " Extension Signature. Error: " + ex.Message);
            }
        }

        private void ScanHex(string strDirectory, string strFile)
        {
            string strHex = null;
            try
            {
                using (FileStream oFileStream = new FileStream(strFile, FileMode.Open))
                {
                    MSSPLogger.WriteToLog(LogPath, "Examining: " + strFile.Substring(strFile.LastIndexOf('\\') + 1));

                    byte[] arrBytes = new byte[oFileStream.Length];
                    oFileStream.Read(arrBytes, 0, int.Parse(oFileStream.Length.ToString()));
                    strHex = BitConverter.ToString(arrBytes);

                    foreach (var signature in VirusSignatures.Signatures)
                    {
                        if (strHex.Contains(signature.SIGNATURE_STRING))
                        {
                            MSSPLogger.WriteToLog(LogPath, "Virus with ID:" + signature.SIGNATURE_ID + " And Signature:" + signature.SIGNATURE_STRING + " Detected in File:" + strFile.Substring(strFile.LastIndexOf('\\') + 1));

                            if (null != VirusDetectedHandler)
                                Context.Invoke(VirusDetectedHandler, strFile.Substring(strFile.LastIndexOf('\\') + 1), strDirectory);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MSSPLogger.WriteToLog(LogPath, "Error Examining " + strFile.Substring(strFile.LastIndexOf('\\') + 1) + " for Virus Signatures. Error: " + ex.Message);
            }
        }
    }
}
