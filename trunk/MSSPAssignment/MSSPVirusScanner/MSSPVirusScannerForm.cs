using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MSSPVirusScanner
{
    public partial class MSSPVirusScannerForm : Form
    {
        private static string LOG_PATH;
        private static int intFileCount = 0;
        private static int intDirCount = 0;
        private static Stopwatch mainTimer;

        public MSSPVirusScannerForm()
        {
            InitializeComponent();
            PopulateTreeView();

            this.tvDirectories.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.tvDirectories_NodeMouseClick);
        }

        private void PopulateTreeView()
        {
            TreeNode tnRoot;

            foreach (var drive in DriveInfo.GetDrives())
            {
                DirectoryInfo oDirInfo = new DirectoryInfo(drive.Name);

                try
                {
                    if (oDirInfo.Exists)
                    {
                        tnRoot = new TreeNode(oDirInfo.Name);
                        tnRoot.Tag = oDirInfo;
                        GetDirectories(oDirInfo.GetDirectories(), tnRoot);
                        tvDirectories.Nodes.Add(tnRoot);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void GetDirectories(DirectoryInfo[] arrSubDirs, TreeNode oParentNode)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;

            foreach (DirectoryInfo subDir in arrSubDirs)
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Nodes.Add("");
                aNode.Tag = subDir;
                aNode.ImageKey = "folder";

                subSubDirs = subDir.GetDirectories("", SearchOption.TopDirectoryOnly);

                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, aNode);
                }

                oParentNode.Nodes.Add(aNode);
            }
        }

        void tvDirectories_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            e.Node.Nodes.Clear();

            try
            {
                GetDirectories(new DirectoryInfo(newSelected.FullPath).GetDirectories(), newSelected);
            }
            catch (Exception ex)
            {
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(InitiateScan);
        }

        public void InitiateScan()
        {
            LOG_PATH = string.Format("C:\\Work\\ScanLog_{0}_{1}.txt", DateTime.Now.Ticks, DateTime.Now.ToString("yyyyMMdd"));
            string strDirectoryToScan = tvDirectories.SelectedNode.FullPath;

            mainTimer = new Stopwatch();
            mainTimer.Start();
            RecurseDirectory(strDirectoryToScan);
            mainTimer.Stop();

            lblText.Text = "Scan of " + strDirectoryToScan + " completed.";
            WriteToLog("Scan of " + strDirectoryToScan + " completed.");

            lblText.Text = intDirCount + " directories and " + intFileCount + " files scanned in " + mainTimer.Elapsed;
            WriteToLog(intDirCount + " directories and " + intFileCount + " files scanned in " + mainTimer.Elapsed);
        }

        public void RecurseDirectory(string strDirectory)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(strDirectory);

                if (dirs.Length == 0)
                {
                    Scan(strDirectory);
                }
                else
                {
                    foreach (var dir in dirs)
                    {
                        RecurseDirectory(dir);
                        intDirCount++;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Scan(string strDirectory)
        {
            Stopwatch dirTimer = new Stopwatch();
            dirTimer.Start();

            string[] files = Directory.GetFiles(strDirectory);

            if (0 == files.Length)
            {
                WriteToLog("No executables found in " + strDirectory);
            }
            else
            {
                foreach (var file in files)
                {
                    lblText.Text = "Scanning Directory: " + strDirectory + "\n";
                    WriteToLog("Scanning Directory: " + strDirectory);

                    lblText.Text = "Scanning File: " + file;
                    WriteToLog("Scanning File: " + file);

                    lblText.Text = "Elapsed Time: " + mainTimer.ElapsedMilliseconds + "\n";
                    WriteToLog("Elapsed Time: " + mainTimer.ElapsedMilliseconds);

                    string hex = GetHexString(file);

                    if (null != hex)
                    {
                        if (hex.StartsWith(FileExtensionTypes.EXE))
                        {
                            WriteToLog("EXE!");
                        }
                        else if (hex.StartsWith(FileExtensionTypes.MSI))
                        {
                            WriteToLog("MSI!");
                        }
                    }
                    intFileCount++;
                }
                dirTimer.Stop();
                WriteToLog("Scan of " + strDirectory);
            }
        }

        public static string GetHexString(string strFile)
        {
            try
            {
                using (FileStream oFileStream = new FileStream(strFile, FileMode.Open))
                {
                    byte[] arrBytes = new byte[oFileStream.Length];

                    oFileStream.Read(arrBytes, 0, int.Parse(oFileStream.Length.ToString()));
                    return BitConverter.ToString(arrBytes);
                }
            }
            catch (Exception ex)
            {
                WriteToLog("Error scanning " + strFile + ". " + ex.Message);
                return null;
            }
        }

        private static void WriteToLog(string strLogEntry)
        {
            using (StreamWriter stream = new StreamWriter(LOG_PATH, true))
            {
                stream.WriteLine(strLogEntry);
            }
        }
    }
}
