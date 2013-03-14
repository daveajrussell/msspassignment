using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace MaliciousProgram
{
    /// <summary>
    /// Small program created for the sole purpose of 'hooking' into
    /// to observe the program's behaviour.
    /// 
    /// This program opens and writes to a test file every 10 seconds
    /// </summary>
    class MaliciousProgram
    {
        /// <summary>
        /// Main function
        /// 
        /// Every 10 seconds a file is opened for writing,
        /// invoking the system32 call.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            while (true)
            {
                Thread.Sleep(10000);

                try
                {
                    using (var fs = File.OpenWrite("C:\\Test.txt"))
                    {
                        using (var sw = new StreamWriter(fs))
                        {
                            sw.WriteLine(DateTime.Now);
                        }
                    }
                }
                // An exception will be thrown if this process is not started with admin rights.
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
