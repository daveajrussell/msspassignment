using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSSPVirusScanner;
using Moq;
using MSSPVirusScanner.Interfaces;
using MSSPVirusScanner.MSSPScanStrategies;
using System.IO;
using System.Text;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using MSSPVirusScanner.Utils;

namespace MSSPVirusScannerTest
{
    /// <summary>
    /// Test class listing for testing the three
    /// strategies, that were later used for load testing.
    /// </summary>
    [TestClass]
    public class StrategyTest
    {
        private MSSPSignatureAnaylsisStrategyContext _boyerMooreStrategyContext;
        private MSSPSignatureAnaylsisStrategyContext _ContainsStrategyContext;
        private MSSPSignatureAnaylsisStrategyContext _indexOfStrategyContext;

        private string[] _signatures;

        private string _file1MB;
        private string _file10MB;
        private string _file25MB;
        private string _file50MB;
        private string _file100MB;

        /// <summary>
        /// Initialise the tests
        /// define concrete implementations of our algorithms and
        /// an array of signatures to scan for.
        /// Reference a number of files of varying sizes to scan.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            _boyerMooreStrategyContext = new MSSPSignatureAnaylsisStrategyContext(new BoyerMooreAnalysisStrategy());
            _ContainsStrategyContext = new MSSPSignatureAnaylsisStrategyContext(new ContainsAnalysisStrategy());
            _indexOfStrategyContext = new MSSPSignatureAnaylsisStrategyContext(new IndexOfStrategy());

            _signatures = new string[]
            {
                "4322DEE4DC68D1E467F739478B52DDFC",
                "F4E2980113748AE7E84EC4CDBAC1B549",
                "611802CAC60477870624A03EB3FD2A06",
                "4F916A7DD206064E65539751DD0947D9",
                "CA44D37E65FDC10C50B6DD80CEE8F426",
                "6E33500AF92DB5A91BCF34F95819CEAF",
                "89B726C5E503E38C70C05A094F1097DE",
                "83AA2373E27E66A21B5B33E496575E6A",
                "A5A24B4D265C885C9942C66B1E5738D1",
                "882FD8242363B6C3F2653789044A4B24",
                "27BAB4B06A12EA0A830F9B3B23996E3E",
                "30337106AE425971CA986C8CED7CBD13",
                "C2986F8337C1D5E146BB4D55FD4C7B25"
            };

            _file1MB = @"C:\Work\TestFile1Mb.dll";
            _file10MB = @"C:\Work\TestFile10Mb.dll";
            _file25MB = @"C:\Work\TestFile25Mb.dll";
            _file50MB = @"C:\Work\TestFile50Mb.dll";
            _file100MB = @"C:\Work\TestFile100Mb.dll";
        }

        [TestMethod]
        public void TestBoyerMooreStrategy1MB()
        {
            PerformBoyerMooreScan(_file1MB);
        }

        [TestMethod]
        public void TestBoyerMooreStrategy10MB()
        {
            PerformBoyerMooreScan(_file10MB);
        }

        [TestMethod]
        public void TestBoyerMooreStrategy25MB()
        {
            PerformBoyerMooreScan(_file25MB);
        }

        [TestMethod]
        public void TestBoyerMooreStrategy50MB()
        {
            PerformBoyerMooreScan(_file50MB);
        }

        [TestMethod]
        public void TestBoyerMooreStrategy100MB()
        {
            PerformBoyerMooreScan(_file100MB);
        }

        [TestMethod]
        public void TestContainsStrategy1MB()
        {
            PerformContainsScan(_file1MB);
        }

        [TestMethod]
        public void TestContainsStrategy10MB()
        {
            PerformContainsScan(_file10MB);
        }

        [TestMethod]
        public void TestContainsStrategy25MB()
        {
            PerformContainsScan(_file25MB);
        }

        [TestMethod]
        public void TestContainsStrategy50MB()
        {
            PerformContainsScan(_file50MB);
        }

        [TestMethod]
        public void TestContainsStrategy100MB()
        {
            PerformContainsScan(_file100MB);
        }

        [TestMethod]
        public void TestIndexOfStrategy1MB()
        {
            PerformIndexOfScan(_file1MB);
        }

        [TestMethod]
        public void TestIndexOfStrategy10MB()
        {
            PerformIndexOfScan(_file10MB);
        }

        [TestMethod]
        public void TestIndexOfStrategy25MB()
        {
            PerformIndexOfScan(_file25MB);
        }

        [TestMethod]
        public void TestIndexOfStrategy50MB()
        {
            PerformIndexOfScan(_file50MB);
        }

        [TestMethod]
        public void TestIndexOfStrategy100MB()
        {
            PerformIndexOfScan(_file100MB);
        }

        /// <summary>
        /// Function to perform a boyer moore scan against the given file
        /// </summary>
        /// <param name="file">The file in which to open into a stream of bytes and analyse the hex</param>
        private void PerformBoyerMooreScan(string file)
        {
            try
            {
                string strHex = null;
                using (FileStream oFileStream = new FileStream(file, FileMode.Open))
                {
                    byte[] arrBytes = new byte[oFileStream.Length];
                    oFileStream.Read(arrBytes, 0, int.Parse(oFileStream.Length.ToString()));
                    strHex =  MSSPUtils.ByteArrayToString(arrBytes);

                    foreach (var signature in _signatures)
                    {
                        _boyerMooreStrategyContext.AnalyseSignature(signature, strHex);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Function to perform a String.Contains scan
        /// </summary>
        /// <param name="file">The given file to scan the hex of</param>
        private void PerformContainsScan(string file)
        {
            try
            {
                string strHex = null;
                using (FileStream oFileStream = new FileStream(file, FileMode.Open))
                {
                    byte[] arrBytes = new byte[oFileStream.Length];
                    oFileStream.Read(arrBytes, 0, int.Parse(oFileStream.Length.ToString()));
                    strHex = MSSPUtils.ByteArrayToString(arrBytes);

                    foreach (var signature in _signatures)
                    {
                        _ContainsStrategyContext.AnalyseSignature(signature, strHex);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Performs a String.IndexOf scan against the file
        /// </summary>
        /// <param name="file">The file to scan</param>
        private void PerformIndexOfScan(string file)
        {
            try
            {
                string strHex = null;
                using (FileStream oFileStream = new FileStream(file, FileMode.Open))
                {
                    byte[] arrBytes = new byte[oFileStream.Length];
                    oFileStream.Read(arrBytes, 0, int.Parse(oFileStream.Length.ToString()));
                    strHex = MSSPUtils.ByteArrayToString(arrBytes);

                    foreach (var signature in _signatures)
                    {
                        _indexOfStrategyContext.AnalyseSignature(signature, strHex);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
