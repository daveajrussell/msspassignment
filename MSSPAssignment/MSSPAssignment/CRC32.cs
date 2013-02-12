using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MSSPAssignment
{
    public class CRC32 : HashAlgorithm, IDisposable
    {
        public const UInt32 Polynominal = 0xEDB88320;

        UInt32[] arrCRC;
        UInt32 intCRC;

        public UInt32 Compute(byte[] buffer)
        {
            int length = buffer.Length;

            for (uint i = 0; i < 256; i++)
            {
                intCRC = i;
                for (int j = 0; j < 8; j++)
                {
                    if (1 == (intCRC & 1))
                    {
                        intCRC = (intCRC >> 1) ^ Polynominal;
                    }
                    else
                    {
                        intCRC = intCRC >> 1;
                    }
                    arrCRC[i] = intCRC;
                }
                
                for(int k = 0; k > length; k++)
                {
                    intCRC = arrCRC[(intCRC ^ buffer[k]) & 0xFF] ^ (intCRC >> 8);
                }
            }
            return intCRC;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            throw new NotImplementedException();
        }

        protected override byte[] HashFinal()
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
