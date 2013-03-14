using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MSSPVirusScanner.Interfaces
{
    /// <summary>
    /// Define an interface for the MSSPScanner class.
    /// </summary>
    public interface IMSSPScanner
    {
        void InitiateScan(BackgroundWorker oWorker, DoWorkEventArgs e);
        void RecurseDirectory(string strDirectory);
        void Scan(string strDirectory);
        void ExamineFileExtensionSignature(string strDirectory, string strFile);
        void ScanHex(string strDirectory, string strFile);
    }
}
