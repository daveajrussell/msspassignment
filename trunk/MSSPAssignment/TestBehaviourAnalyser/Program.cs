using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyHook;
using System.Runtime.Remoting;
using System.Diagnostics;

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
                    //if(proc.
                    try
                    {
                        RemoteHooking.Inject(
                           proc.Id,
                           @"FileMonInject.dll",
                           @"FileMonInject.dll",
                           ChannelName);
                    }
                    catch (Exception ex)
                    {
                        // do not raise
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
