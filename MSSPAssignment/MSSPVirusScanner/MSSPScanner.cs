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
using MSSPVirusScanner.MSSPScanStrategies;
using MSSPVirusScanner.Interfaces;
using MSSPVirusScanner.Utils;

namespace MSSPVirusScanner
{
    /// <summary>
    /// Class for the main scanner, implements the Scanner interface.
    /// This class is where a great deal of the application work occurs.
    /// </summary>
    public class MSSPScanner : IMSSPScanner
    {
        private string LogPath { get; set; }
        private BackgroundWorker Worker { get; set; }
        private DoWorkEventArgs WorkerEvent { get; set; }
        private MSSPVirusScannerForm Context { get; set; }
        private MSSPSignatureAnaylsisStrategyContext mScanStrategyContext;
        private MSSPFileSignatureAnalysisStrategyContext mIdentificationStrategyContext;
        private MSSPSignatureDatabase Signatures;
        private Signatures VirusSignatures { get; set; }

        private static int intAccumulatingFileCount;
        private static int intTotalFileCount;
        private static int intDirectoryCount;
        private static Stopwatch mainTimer;

        // Define a set of delegates and their event handlers to bubble information back to the calling application
        public delegate void ProgressUpdateDelegate(string strDirectory, string strFile, string strFileCount);
        public delegate void ScanCompleteDelegate(string strDirectory, string strDirectoryCount, string strFileCount);
        public delegate void VirusDetectedDelegate(string strDirectory, string strFile, string strVirusName);

        public event ProgressUpdateDelegate ProgressUpdateHandler;
        public event ScanCompleteDelegate ScanCompleteHandler;
        public event VirusDetectedDelegate VirusDetectedHandler;

        /// <summary>
        /// Class constructor for initialising all class variables before the
        /// BackgroundWorker DoWork is invoked.
        /// </summary>
        /// <param name="strLogPath">The path of the log file</param>
        /// <param name="oMSSPVirusScannerForm">The calling context</param>
        /// <param name="oStrategy">The concreate signature scanning strategy to use</param>
        /// <param name="oIdentificationStrategy">The concrete file signature strategy to use</param>
        public MSSPScanner(string strLogPath, MSSPVirusScannerForm oMSSPVirusScannerForm, MSSPSignatureAnalysisStrategy oStrategy, MSSPFileSignatureAnalysisStrategy oIdentificationStrategy)
        {
            intAccumulatingFileCount = 1;
            intTotalFileCount = 1;
            intDirectoryCount = 1;

            LogPath = strLogPath;
            Context = oMSSPVirusScannerForm;

            // Get the virus signatures from the database
            Signatures = new MSSPSignatureDatabase();
            VirusSignatures = Signatures.GetKnownSignatures();

            this.mScanStrategyContext = new MSSPSignatureAnaylsisStrategyContext(oStrategy);
            this.mIdentificationStrategyContext = new MSSPFileSignatureAnalysisStrategyContext(oIdentificationStrategy);
        }

        /// <summary>
        /// Initiate Scan invoked by the BackgroundWoker DoWork method
        /// </summary>
        /// <param name="oWorker">The invoking worker context</param>
        /// <param name="e">Any arguments passed to the worker, in this case the argument is the directory to initially scan</param>
        public void InitiateScan(BackgroundWorker oWorker, DoWorkEventArgs e)
        {
            Worker = oWorker;
            WorkerEvent = e;

            // Begin a stopwatch so we know how long the scan took.
            mainTimer = new Stopwatch();
            mainTimer.Start();
            RecurseDirectory(e.Argument.ToString());
            mainTimer.Stop();

            // Upon completion, write all information to the log
            MSSPLogger.WriteToLog(LogPath, "Scan of " + e.Argument.ToString() + " Completed.");
            MSSPLogger.WriteToLog(LogPath, intDirectoryCount + " Directories and " + intTotalFileCount + " Files Scanned in " + mainTimer.Elapsed);

            // Invoke the scan complete handler so the main activity can display an idication
            if (null != ScanCompleteHandler)
            {
                Context.Invoke(ScanCompleteHandler, e.Argument.ToString(), intDirectoryCount.ToString(), intTotalFileCount.ToString());
            }

            // Close the connection to the database
            Signatures.Close();
        }

        /// <summary>
        /// Recursively scan the directory for all directories and files.
        /// </summary>
        /// <param name="strDirectory">The directory to recurse</param>
        public void RecurseDirectory(string strDirectory)
        {
            // Check if the Background Worker has been cancelled.
            if (Worker.CancellationPending)
                Signatures.Close();
            else
            {
                try
                {
                    // Get all directories for this directory
                    string[] dirs = Directory.GetDirectories(strDirectory);

                    if (0 == dirs.Length)
                    {
                        // If there are no directories, scan the files in the current directory
                        Scan(strDirectory);
                    }
                    else
                        foreach (var dir in dirs)
                        {
                            //Otherwise, recurse through each of the directories in this directory
                            RecurseDirectory(dir);
                            // Keep a running total of directories recursed.
                            intDirectoryCount++;
                        }
                }
                catch (Exception ex)
                {
                    // In the event of a scan failing, write to the log.
                    // Most commonly these are permission denied errors.
                    MSSPLogger.WriteToLog(LogPath, "Error scanning " + strDirectory + ". " + ex.Message);

                    // In case something went wrong with the thread, abandon the connection to the database so it is not left open
                    if (ex is ThreadAbortException || ex is OutOfMemoryException)
                        Signatures.Close();
                }
            }
        }

        /// <summary>
        /// Function to scan each of the files within a directory
        /// </summary>
        /// <param name="strDirectory">The directory for which to scan</param>
        public void Scan(string strDirectory)
        {
            // Get a collection of the files
            string[] files = Directory.GetFiles(strDirectory);

            if (0 != files.Length)
            {
                // Iterate the collection of files
                foreach (var strFile in files)
                {
                    if (null != ProgressUpdateHandler)
                        // Update the main application indicating the current directory, file and the total file count
                        Context.Invoke(ProgressUpdateHandler, strDirectory, strFile, intTotalFileCount.ToString());

                    // Calculate the percentage of the files scanned and report the progress (updates the progress bar)
                    int intPercentComplete = (int)((float)intAccumulatingFileCount / (float)files.Length * 100);
                    Worker.ReportProgress(intPercentComplete);

                    // Call the function to examine the file
                    ExamineFileExtensionSignature(strDirectory, strFile);
                    
                    // Keep a running total
                    intAccumulatingFileCount++;
                    intTotalFileCount++;
                }
            }
            // Reset the file count
            intAccumulatingFileCount = 0;

        }

        /// <summary>
        /// A function to examine a file for a particular signature
        /// </summary>
        /// <param name="strDirectory">The directory the file belongs to</param>
        /// <param name="strFile">The file to examine the file signature for</param>
        public void ExamineFileExtensionSignature(string strDirectory, string strFile)
        {
            string strHex = null;
            // We want to keep this as efficient as possible, 
            // the largest file extension is 66 bytes long
            byte[] arrBytes = new byte[66];

            try
            {
                // Open the file into a stream to analyse the bytes
                using (FileStream oFileStream = new FileStream(strFile, FileMode.Open))
                {
                    oFileStream.Read(arrBytes, 0, 66);
                    // Conert to hex
                    strHex = MSSPUtils.ByteArrayToString(arrBytes);
                }

                if (null != strHex)
                    // Call the strategy context to scan the hex.
                    if (mIdentificationStrategyContext.AnalyseFileSignature(strHex))
                        // The the file is a type we want to scan, call the ScanHex function.
                        ScanHex(strDirectory, strFile);
            }
            catch (Exception ex)
            {
                MSSPLogger.WriteToLog(LogPath, "Error Examining " + strFile.Substring(strFile.LastIndexOf('\\') + 1) + " Extension Signature. Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Function to scan the entire hex string of the file
        /// </summary>
        /// <param name="strDirectory">The directory the file belongs to</param>
        /// <param name="strFile">The file to scan</param>
        public void ScanHex(string strDirectory, string strFile)
        {
            string strHex = null;
            try
            {
                // Open the file into a stream of bytes
                using (FileStream oFileStream = new FileStream(strFile, FileMode.Open))
                {
                    MSSPLogger.WriteToLog(LogPath, "Examining: " + strFile.Substring(strFile.LastIndexOf('\\') + 1));

                    // Convert the bytes to a string of hex
                    byte[] arrBytes = new byte[oFileStream.Length];
                    oFileStream.Read(arrBytes, 0, int.Parse(oFileStream.Length.ToString()));
                    strHex = MSSPUtils.ByteArrayToString(arrBytes);

                    // Iterate through all of the virus signatures
                    foreach (var signature in VirusSignatures.Items)
                    {
                        // Call the analyse signature function for this signature
                        if (mScanStrategyContext.AnalyseSignature(signature.String, strHex))
                        {
                            // if a virus was detected, write to the log
                            MSSPLogger.WriteToLog(LogPath, "Virus with ID:" + signature.ID + " And Signature:" + signature.String + " Detected in File:" + strFile.Substring(strFile.LastIndexOf('\\') + 1));

                            // Also invoke the handler to bubble the event to the calling application.
                            if (null != VirusDetectedHandler)
                                Context.Invoke(VirusDetectedHandler, strDirectory, strFile.Substring(strFile.LastIndexOf('\\') + 1), signature.Name);
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
