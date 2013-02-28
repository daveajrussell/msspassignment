using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyHook;
using System.Runtime.Remoting;
using System.Diagnostics;
using System.Management;

namespace FileMon
{
    public class FileMonInterface : MarshalByRefObject
    {
        public void IsInstalled(Int32 intClientPID)
        {
            Console.WriteLine("FileMon has been installed in target {0}. \r\n", intClientPID);
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

    class Program
    {
        static String ChannelName = null;

        static void Main(string[] args)
        {
            try
            {
                Config.Register(
                    "A FileMon like demo application.",
                    @"FileMon.exe",
                    @"FileMonInject.dll");

                RemoteHooking.IpcCreateServer<FileMonInterface>(
                    ref ChannelName,
                    WellKnownObjectMode.SingleCall);

                foreach (var proc in Process.GetProcesses())
                {
                    string strQuery = string.Format("Select * from Win32_Process Where ProcessID = {0}", proc.Id);
                    ManagementObjectSearcher search = new ManagementObjectSearcher(strQuery);
                    ManagementObjectCollection processList = search.Get();

                    foreach (ManagementObject obj in processList)
                    {
                        string[] argList = new string[] { string.Empty, string.Empty };
                        obj.InvokeMethod("GetOwner", argList);

                        if ("SYSTEM" != argList[1])
                        {
                            RemoteHooking.Inject(
                               proc.Id,
                               @"FileMonInject.dll",
                               @"FileMonInject.dll",
                               ChannelName);
                        }
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
