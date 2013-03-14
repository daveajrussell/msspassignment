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
    /// <summary>
    /// 
    /// </summary>
    public class Main : IEntryPoint
    {
        MSSPBehaviourMonitorInterface Interface;
        LocalHook CreateFileHook;
        Stack<String> Queue = new Stack<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InContext"></param>
        /// <param name="InChannelName"></param>
        public Main(RemoteHooking.IContext InContext, String InChannelName)
        {
            Interface = RemoteHooking.IpcConnectClient<MSSPBehaviourMonitorInterface>(InChannelName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InContext"></param>
        /// <param name="InChannelName"></param>
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
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InFileName"></param>
        /// <param name="InDesiredAccess"></param>
        /// <param name="InSharedMode"></param>
        /// <param name="InSecurityAttributes"></param>
        /// <param name="InCreationDisposition"></param>
        /// <param name="InFlagsAndAttributes"></param>
        /// <param name="InTemplateFile"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InFileName"></param>
        /// <param name="InDesiredAccess"></param>
        /// <param name="InShareMode"></param>
        /// <param name="InSecurityAttributes"></param>
        /// <param name="InCreationDisposition"></param>
        /// <param name="InFlagsAndAttributes"></param>
        /// <param name="InTemplateFile"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InFileName"></param>
        /// <param name="InDesiredAccess"></param>
        /// <param name="InShareMode"></param>
        /// <param name="InSecurityAttributes"></param>
        /// <param name="InCreationDisposition"></param>
        /// <param name="InFlagsAndAttributes"></param>
        /// <param name="InTemplateFile"></param>
        /// <returns></returns>
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
