using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyHook;
using System.Runtime.Remoting;
using System.Diagnostics;
using System.Management;

namespace MSSPBehaviourMonitor
{
    public class MSSPBehaviourMonitorInterface : MarshalByRefObject
    {
        public void IsInstalled(Int32 intClientPID)
        {
            Console.WriteLine("Behaviour of Process {0} is now being monitored\r\n", intClientPID);
        }

        public void OnCreateFile(Int32 intClientPID, String[] arrFileNames)
        {
            for (int i = 0; i < arrFileNames.Length; i++)
            {
                Console.WriteLine(arrFileNames[i]);
            }
        }

        public void ReportException(Exception ex)
        {
            Console.WriteLine("The target process has reported an error: {0}\r\n", ex.ToString());
            Console.ReadLine();
        }

        public void Ping()
        {
        }
    }

    class MSSPBehaviourMonitor
    {
        static String ChannelName = null;

        static void Main(string[] args)
        {
            try
            {
                Config.Register(
                    "MSSP Behaviour Monitor",
                    @"MSSPBehaviourMonitor.exe",
                    @"MSSPMonitorInject.dll");

                RemoteHooking.IpcCreateServer<MSSPBehaviourMonitorInterface>(
                    ref ChannelName,
                    WellKnownObjectMode.SingleCall);

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
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an error while connecting to target:\r\n{0}", ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
