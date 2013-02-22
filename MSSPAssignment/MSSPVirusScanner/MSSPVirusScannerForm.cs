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
        private string LogPath { get; set; }

        private Scanner mScanner;
        private Thread mScanThread;
        private Stopwatch mProgramTimer;

        public MSSPVirusScannerForm()
        {
            InitializeComponent();
            PopulateTreeView();

            this.tvDirectories.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.tvDirectories_NodeMouseClick);
            this.btnCancel.Click += btnCancel_Click;
            this.btnScan.Click += btnScan_Click;
            this.tickTimer.Tick += tickTimer_Tick;

            this.bgScanner.DoWork += bgScanner_DoWork;
            this.bgScanner.ProgressChanged += bgScanner_ProgressChanged;
            this.bgScanner.RunWorkerCompleted += bgScanner_RunWorkerCompleted;

            this.bgScanner.WorkerReportsProgress = true;
            this.bgScanner.WorkerSupportsCancellation = true;

            this.mProgramTimer = new Stopwatch();
        }

        private void tickTimer_Tick(object sender, EventArgs e)
        {
            txtElapsedTime.Text = (mProgramTimer.ElapsedMilliseconds / 1000).ToString();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            txtStartTime.Text = DateTime.Now.ToShortTimeString();
            tickTimer.Start();
            mProgramTimer.Start();

            this.btnCancel.Enabled = true;
            this.btnScan.Enabled = false;

            LogPath = string.Format("C:\\Work\\ScanLogs\\ScanLog_{0}_{1}.txt", DateTime.Now.Ticks, DateTime.Now.ToString("yyyyMMdd"));

            /*
            string strDirectoryToScan = tvDirectories.SelectedNode.FullPath;
            mScanThread = new Thread(() => mScanner.InitiateScan(strDirectoryToScan));
            mScanThread.Start();
            */

            mScanner = new Scanner(LogPath, this);
            mScanner.ProgressUpdateHandler += mScanner_ProgressUpdateHandler;
            mScanner.ScanCompleteHandler += mScanner_ScanCompleteHandler;
            mScanner.VirusDetectedHandler += mScanner_VirusDetectedHandler;

            string strDirectoryToScan = tvDirectories.SelectedNode.FullPath;
            bgScanner.RunWorkerAsync(strDirectoryToScan);
        }

        private void bgScanner_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker oWorker = sender as BackgroundWorker;
            mScanner.InitiateScan(oWorker, e);
        }

        private void bgScanner_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void bgScanner_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //mScanThread.Abort();
            bgScanner.CancelAsync();
            bgScanner.Dispose();
            tickTimer.Stop();
            mProgramTimer.Stop();
            mProgramTimer.Reset();

            this.progressBar1.Value = 0;
            this.btnCancel.Enabled = false;
            this.btnScan.Enabled = true;

            mScanner = null;

            /*txtCurrentDir.Text = "";
            txtCurrentFile.Text = "";
            txtItemsScanned.Text = "";
            txtElapsedTime.Text = "Scan cancelled at " + DateTime.Now.ToShortTimeString();*/

            Logger.WriteToLog(LogPath, "Scan Cancelled by User");
        }

        private void mScanner_ProgressUpdateHandler(string strDirectory, string strFile, string strFileCount)
        {
            txtCurrentDir.Text = strDirectory;
            txtCurrentFile.Text = strFile;
            txtItemsScanned.Text = strFileCount;
        }

        private void mScanner_ScanCompleteHandler(string strDirectory, string strDirectoryCount, string strFileCount, long lngElapsedMillis)
        {
            tickTimer.Stop();
            mProgramTimer.Stop();
            mProgramTimer.Reset();

            txtCurrentDir.Text = "Scan of " + strDirectory + " Complete!";
            txtCurrentFile.Text = "";
            txtItemsScanned.Text = strFileCount + " Total Files Scanned!";
            txtElapsedTime.Text = (lngElapsedMillis / 1000 * 60).ToString() + " Total Elapsed Time!";
        }

        private void mScanner_VirusDetectedHandler(string strDirectory, string strFile)
        {
            MessageBox.Show("Virus Detected in: " + strFile);
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
                    Logger.WriteToLog(LogPath, "Error Populating Tree View. Error: " + ex.Message);
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

                if (0 != subSubDirs.Length)
                    GetDirectories(subSubDirs, aNode);

                oParentNode.Nodes.Add(aNode);
            }
        }

        private void tvDirectories_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            e.Node.Nodes.Clear();

            try
            {
                GetDirectories(new DirectoryInfo(newSelected.FullPath).GetDirectories(), newSelected);
            }
            catch (Exception ex)
            {
                Logger.WriteToLog(LogPath, "Error Populating Tree View. Error: " + ex.Message);
            }
        }
    }
}
