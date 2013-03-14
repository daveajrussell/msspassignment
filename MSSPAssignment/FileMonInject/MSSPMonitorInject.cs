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
    /// This class creates the DLL used for hooking into the WIN32 API CreateFileW call.
    /// CreateFileW - Create File for Writing. 
    /// This is very basic behaviour monitoring, it is observing a process opening
    /// a file for writing, an activity that a malcious program may very well do.
    /// 
    /// This code has been distilled from the original article at: http://www.codeproject.com/Articles/27637/EasyHook-The-reinvention-of-Windows-API-hooking
    /// and a much better understanding can be gained from this article.
    /// I'll explain this code as I interpreted it.
    /// </summary>
    public class Main : IEntryPoint
    {
        // Get an instance of the interface we created in the behaviour monitor
        // class so that events from this class can be bubbled up to the
        // main application.
        MSSPBehaviourMonitorInterface Interface;
        LocalHook CreateFileHook;
        Stack<String> Queue = new Stack<string>();

        /// <summary>
        /// function to instantiate the interface with an
        /// interprocess communication client.
        /// </summary>
        /// <param name="InContext"></param>
        /// <param name="InChannelName"></param>
        public Main(RemoteHooking.IContext InContext, String InChannelName)
        {
            Interface = RemoteHooking.IpcConnectClient<MSSPBehaviourMonitorInterface>(InChannelName);
        }

        /// <summary>
        /// Main loop of the class.
        /// </summary>
        /// <param name="InContext"></param>
        /// <param name="InChannelName"></param>
        public void Run(RemoteHooking.IContext InContext, String InChannelName)
        {
            try
            {
                // Install the CreateFileW hook 
                CreateFileHook = LocalHook.Create(
                    LocalHook.GetProcAddress("kernel32.dll", "CreateFileW"),
                    new DCreateFile(CreateFile_Hooked),
                    this);

                CreateFileHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            }
            catch (Exception ex)
            {
                // In the event of an exception, propogate an exception event to the calling 
                // application through the interface
                Interface.ReportException(ex);
                return;
            }

            // When a hook is installed in a process, bubble this event back
            // to the calling application through the interface, passing the 
            // ID of the hooked process
            Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());

            try
            {
                while (true)
                {
                    Thread.Sleep(500);

                    if (Queue.Count > 0)
                    {
                        // Any Open File for Write calls that were ran are added to the Queue,
                        // every 500 ms this process runs and sends them to the 
                        // calling application through the interface
                        String[] Package = null;

                        // Lock the queue to ensure nothing is written to it during the dequeueing process
                        lock (Queue)
                        {
                            Package = Queue.ToArray();
                            Queue.Clear();
                        }

                        // Send the information to the interface
                        Interface.OnCreateFile(RemoteHooking.GetCurrentProcessId(), Package);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Get a pointer reference to the DCreateFile function
        /// so that we may intercept the call in this class.
        /// </summary>
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
        /// As this is C# we need to get native access
        /// to the win32 API.
        /// </summary>
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
        /// It seems that all of the API calls that we've registered to be 
        /// listened to in the hooked process are intercepted and are  ran through
        /// this class, so we need to call the original API
        /// once the call has passed through this class
        /// </summary>
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
