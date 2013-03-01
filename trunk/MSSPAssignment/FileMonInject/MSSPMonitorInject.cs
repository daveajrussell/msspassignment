using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyHook;
using MSSPVirusScanner;
using System.Threading;
using System.Runtime.InteropServices;

namespace FileMonInject
{
    public class Main : IEntryPoint
    {
        MSSPBehaviourMonitorInterface Interface;
        LocalHook CreateFileHook;
        Stack<String> Queue = new Stack<string>();

        public Main(RemoteHooking.IContext InContext, String InChannelName)
        {
            Interface = RemoteHooking.IpcConnectClient<MSSPBehaviourMonitorInterface>(InChannelName);
        }

        public void Run(RemoteHooking.IContext InContext, String InChannelName)
        {
            try
            {
                CreateFileHook = LocalHook.Create(
                    LocalHook.GetProcAddress("kernel32.dll", "CreateFileW"),
                    new DCreateFile(CreateFile_Hooked),
                    this);

                CreateFileHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            }
            catch (Exception ex)
            {
                Interface.ReportException(ex);
                return;
            }

            Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());

            try
            {
                while (true)
                {
                    Thread.Sleep(500);

                    if (Queue.Count > 0)
                    {
                        String[] Package = null;

                        lock (Queue)
                        {
                            Package = Queue.ToArray();
                            Queue.Clear();
                        }

                        Interface.OnCreateFile(RemoteHooking.GetCurrentProcessId(), Package);
                    }
                    else
                    {
                        Interface.Ping();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            SetLastError = true)]
        delegate IntPtr DCreateFile(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InSharedMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile);

        [DllImport("kernel32.dll",
            CharSet = CharSet.Unicode,
            SetLastError = true,
            CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr CreateFile(
              String InFileName,
              UInt32 InDesiredAccess,
              UInt32 InShareMode,
              IntPtr InSecurityAttributes,
              UInt32 InCreationDisposition,
              UInt32 InFlagsAndAttributes,
              IntPtr InTemplateFile);

        static IntPtr CreateFile_Hooked(
            String InFileName,
            UInt32 InDesiredAccess,
            UInt32 InShareMode,
            IntPtr InSecurityAttributes,
            UInt32 InCreationDisposition,
            UInt32 InFlagsAndAttributes,
            IntPtr InTemplateFile)
        {
            try
            {
                Main This = (Main)HookRuntimeInfo.Callback;

                lock (This.Queue)
                {
                    This.Queue.Push(InFileName);
                }
            }
            catch
            {
            }

            return CreateFile(
                InFileName,
                InDesiredAccess,
                InShareMode,
                InSecurityAttributes,
                InCreationDisposition,
                InFlagsAndAttributes,
                InTemplateFile);
        }
    }
}
