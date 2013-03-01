using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace MaliciousProgram
{
    class MaliciousProgram
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Thread.Sleep(10000);

                using (var fs = File.OpenWrite("C:\\Test.txt"))
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(DateTime.Now);
                    }
                }
            }
        }
    }
}
