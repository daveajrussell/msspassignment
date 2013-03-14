using EasyHook;
using MSSPVirusScanner.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;
using System.Threading;

namespace MSSPVirusScanner
{
    /// <summary>
    /// Class to define an interface that the MSSPMonitorInject program 
    /// can use to communicate with the BehaviourMonitor class.
    /// </summary>
    public class MSSPBehaviourMonitorInterface : MarshalByRefObject
    {
        /// <summary>
        /// This function is called when a hook is successfully installed
        /// within a process. This is then used to invoke a method on the behaviour monitor
        /// to bubble the event up to the main application, so the program
        /// being monitored is displayed.
        /// </summary>
        /// <param name="intProcessID">The Id of the process that has been hooked</param>
        public void IsInstalled(Int32 intProcessID)
        {
            MSSPBehaviourMonitor.InvokeOnProcessHooked(intProcessID);
        }

        /// <summary>
        /// This function is called whenever a process that is hooked into
        /// performs a createfile function. This is used to bubble information
        /// up to the main application where it is displayed.
        /// </summary>
        /// <param name="intClientPID">The id of the process that has triggered the event</param>
        /// <param name="arrFileNames">The file names of the affected files that were opened</param>
        public void OnCreateFile(Int32 intClientPID, String[] arrFileNames)
        {
            for (int i = 0; i < arrFileNames.Length; i++)
                MSSPBehaviourMonitor.InvokeOnFileCreated(DateTime.Now, intClientPID, arrFileNames[i]);
        }

        /// <summary>
        /// In case of a remote exception occuring, this function is called
        /// so that the error can be logged and analysd.
        /// </summary>
        /// <param name="ex">The remote exception that occured.</param>
        public void ReportException(Exception ex)
        {
            MSSPBehaviourMonitor.InvokeOnException(ex);
        }
    }

    /// <summary>
    /// Class listing for the behaviour monitor.
    /// This class runs on a BackgroundWorker thread for easy management.
    /// 
    /// The purpose of this class is to hook into our 'malicious process' to
    /// monitor its behaviour - a very rudimentary take on a behaviour scanner
    /// seen in commercial virus scanners.
    /// </summary>
    public class MSSPBehaviourMonitor
    {
        private String ChannelName;
        static string LogPath { get; set; }
        private IpcServerChannel Server;
        private static MSSPVirusScannerForm Context { get; set; }

        public delegate void ProcessHookedDelegate(int intProcessID, string strProcessName);
        public delegate void FileCreateDelegate(DateTime dtNow, int intProcessID, string strFileName);

        public static event ProcessHookedDelegate OnProcessHooked;
        public static event FileCreateDelegate OnFileCreated;

        public static List<int> HookedProcessIDs;

        /// <summary>
        /// Class constructor, this is where the log path and the calling context
        /// is initialised for use throughout the program,
        /// </summary>
        /// <param name="strLogPath">the log path so the app knows where to log events</param>
        /// <param name="oMSSPVirusScannerForm">The calling context, so we can bubble events back up</param>
        public MSSPBehaviourMonitor(string strLogPath, MSSPVirusScannerForm oMSSPVirusScannerForm)
        {
            Context = oMSSPVirusScannerForm;
            LogPath = strLogPath;
            Server = RemoteHooking.IpcCreateServer<MSSPBehaviourMonitorInterface>(ref ChannelName, WellKnownObjectMode.Singleton);
            HookedProcessIDs = new List<int>();
        }

        /// <summary>
        /// Called by the DoWork function of BackgroundWorker.
        /// This runs on a separate thread.
        /// </summary>
        public void InitiateMonitor()
        {
            try
            {
                // Register the calling application and the remote process
                Config.Register(
                    "MSSP Virus Scanner",
                    @"MSSPVirusScanner.exe",
                    @"MSSPMonitorInject.dll");

                // Infinitely loop, to emulate 'real-time' behaviour monitoring
                while (true)
                {
                    // Iterate through each process currently running
                    foreach (var proc in Process.GetProcesses())
                    {
                        // We only want to hook into a process once.
                        if(!HookedProcessIDs.Contains(proc.Id))
                        {
                            // For this application, scope the processes we can hook into
                            // Blue screens can occur upon attempting to hook into certain SYSTEM and NETWORK processes
                            if (proc.ProcessName == "MaliciousProgram.vshost" || proc.ProcessName == "MaliciousProgram")
                            {
                                // attempt to hook into the process using the process ID
                                RemoteHooking.Inject(
                                           proc.Id,
                                           @"MSSPMonitorInject.dll",
                                           @"MSSPMonitorInject.dll",
                                           ChannelName);
                                // If successful, add the ID to the array of hooked processes.
                                HookedProcessIDs.Add(proc.Id);
                            }
                        }
                    }
                    // Wait for 10 seconds
                    Thread.Sleep(10000);
                }
            }
            catch (Exception ex)
            {
                MSSPLogger.WriteToLog(LogPath, string.Format("There was an error while connecting to target:\r\n{0}", ex.ToString()));
            }
        }

        /// <summary>
        /// Function to bubble the FileCreated event back up to the calling application
        /// </summary>
        /// <param name="dtNow">The date and time that the file was created</param>
        /// <param name="intProcessID">The id of the process that created the file</param>
        /// <param name="strFileName">The name of the file that was created</param>
        internal static void InvokeOnFileCreated(DateTime dtNow, int intProcessID, string strFileName)
        {
            if (null != OnFileCreated)
                Context.Invoke(OnFileCreated, dtNow, intProcessID, strFileName);

            MSSPLogger.WriteToLog(LogPath,string.Format("{0}: Process {1} Opened \"{2}\" for Writing\n", dtNow.ToString(), intProcessID.ToString(), strFileName));
        }

        /// <summary>
        /// Function to bubble a ProcessHooked event back up to the caling application.
        /// </summary>
        /// <param name="intProcessID">The id of the process that was hooked in to</param>
        internal static void InvokeOnProcessHooked(int intProcessID)
        {
            Process oProcess = Process.GetProcessById(intProcessID);

            if (null != OnProcessHooked)
                Context.Invoke(OnProcessHooked, oProcess.Id, oProcess.ProcessName);

            MSSPLogger.WriteToLog(LogPath, string.Format("Successfully installed a hook in process with ID: {0}\n", intProcessID));
        }

        /// <summary>
        /// Upon an exception occuring, we want to log the exception.
        /// </summary>
        /// <param name="ex"></param>
        internal static void InvokeOnException(Exception ex)
        {
            MSSPLogger.WriteToLog(LogPath, string.Format("The target process has reported an error: {0}", ex.Message));
        }
    }
}
