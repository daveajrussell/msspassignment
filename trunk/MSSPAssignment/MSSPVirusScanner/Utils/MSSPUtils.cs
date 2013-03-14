using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;

namespace MSSPVirusScanner.Utils
{
    /// <summary>
    /// Class containing commonly occuring utilities for the application
    /// </summary>
    public static class MSSPUtils
    {
        /// <summary>
        /// Originally, BitConverter.ToString was used to convert an
        /// array of bytes to a string of hex. However this was causing
        /// OutOfMemoryExceptions to be thrown when testing the 100MB files.
        /// This method was found which is much faster than BitConverter.
        /// </summary>
        /// <param name="arrBytes">The array of bytes to convert to a string of Hex</param>
        /// <returns>A string of Hex representing the bytes</returns>
        public static string ByteArrayToString(byte[] arrBytes)
        {
            SoapHexBinary shb = new SoapHexBinary(arrBytes);
            return shb.ToString();
        }
    }
}
