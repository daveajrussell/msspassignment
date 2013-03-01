using EasyHook;
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
    public class MSSPBehaviourMonitorInterface : MarshalByRefObject
    {
        public void IsInstalled(Int32 intProcessID)
        {
            MSSPBehaviourMonitor.InvokeOnProcessHooked(intProcessID);
        }

        public void OnCreateFile(Int32 intClientPID, String[] arrFileNames)
        {
            for (int i = 0; i < arrFileNames.Length; i++)
                MSSPBehaviourMonitor.InvokeOnFileCreated(DateTime.Now, intClientPID, arrFileNames[i]);
        }

        public void ReportException(Exception ex)
        {
            MSSPBehaviourMonitor.InvokeOnException(ex);
        }

        public void Ping()
        {
        }
    }

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

        public MSSPBehaviourMonitor(string strLogPath, MSSPVirusScannerForm oMSSPVirusScannerForm)
        {
            Context = oMSSPVirusScannerForm;
            LogPath = strLogPath;
            Server = RemoteHooking.IpcCreateServer<MSSPBehaviourMonitorInterface>(ref ChannelName, WellKnownObjectMode.Singleton);

            try
            {
                Config.Register(
                    "MSSP Virus Scanner",
                    @"MSSPVirusScanner.exe",
                    @"MSSPMonitorInject.dll");

                foreach (var proc in Process.GetProcesses())
                {
                    if (proc.ProcessName == "MaliciousProgram.vshost" || proc.ProcessName == "MaliciousProgram")
                    {
                        RemoteHooking.Inject(
                                   proc.Id,
                                   @"MSSPMonitorInject.dll",
                                   @"MSSPMonitorInject.dll",
                                   ChannelName);
                    }
                }
            }
            catch (Exception ex)
            {
                MSSPLogger.WriteToLog(LogPath, string.Format("There was an error while connecting to target:\r\n{0}", ex.ToString()));
            }
        }

        public void StopMonitoring()
        {
            Server.StopListening(null);
        }

        internal static void InvokeOnFileCreated(DateTime dtNow, int intProcessID, string strFileName)
        {
            if (null != OnFileCreated)
                Context.Invoke(OnFileCreated, dtNow, intProcessID, strFileName);
        }

        internal static void InvokeOnProcessHooked(int intProcessID)
        {
            Process oProcess = Process.GetProcessById(intProcessID);

            if (null != OnProcessHooked)
                Context.Invoke(OnProcessHooked, oProcess.Id, oProcess.ProcessName);
        }

        internal static void InvokeOnException(Exception ex)
        {
            MSSPLogger.WriteToLog(LogPath, string.Format("The target process has reported an error: {0}", ex.Message));
        }
    }
}
