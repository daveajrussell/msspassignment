﻿using System;
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

        private MSSPScanner mScanner;
        private MSSPBehaviourMonitor mMonitor;
        private Stopwatch mProgramTimer;

        public MSSPVirusScannerForm()
        {
            InitializeComponent();
            PopulateTreeView();

            StartBehaviourMonitor();

            this.tvDirectories.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.tvDirectories_NodeMouseClick);
            this.btnCancel.Click += btnCancel_Click;
            this.btnScan.Click += btnScan_Click;
            this.tickTimer.Tick += tickTimer_Tick;

            this.bgScanner.DoWork += bgScanner_DoWork;
            this.bgScanner.ProgressChanged += bgScanner_ProgressChanged;

            this.bgScanner.WorkerReportsProgress = true;
            this.bgScanner.WorkerSupportsCancellation = true;

            this.mProgramTimer = new Stopwatch();
        }

        private void tickTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan time = mProgramTimer.Elapsed;
            txtElapsedTime.Text = string.Format("{0}:{1}:{2}", time.Hours <= 0 ? "00" : time.Hours < 10 ? "0" + time.Hours.ToString() : time.Hours.ToString(), 
                                                               time.Minutes <= 0 ? "00" : time.Minutes < 10 ? "0" + time.Minutes.ToString() : time.Minutes.ToString(), 
                                                               time.Seconds <= 0 ? "00" : time.Seconds < 10 ? "0" + time.Seconds.ToString() : time.Seconds.ToString());
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if (null == tvDirectories.SelectedNode)
            {
                MessageBox.Show("Select a Directory to Scan");
                return;
            }

            txtStartTime.Text = DateTime.Now.ToShortTimeString();
            tickTimer.Start();
            mProgramTimer.Start();

            this.btnCancel.Enabled = true;
            this.btnScan.Enabled = false;

            this.txtCurrentDir.Text = "";
            this.txtCurrentFile.Text = "";
            this.txtItemsScanned.Text = "";
            this.txtElapsedTime.Text = "";

            LogPath = string.Format("C:\\Work\\ScanLogs\\ScanLog_{0}_{1}.txt", DateTime.Now.Ticks, DateTime.Now.ToString("yyyyMMdd"));

            mScanner = new MSSPScanner(LogPath, this);
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

        private void bgScanner_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (100 >= e.ProgressPercentage)
                this.progressBar1.Value = e.ProgressPercentage;
        }

        private void StartBehaviourMonitor()
        {
            mMonitor = new MSSPBehaviourMonitor(LogPath, this);
            
            MSSPBehaviourMonitor.OnFileCreated += MSSPBehaviourMonitor_OnFileCreated;
            MSSPBehaviourMonitor.OnProcessHooked += MSSPBehaviourMonitor_OnProcessHooked;
        }

        public void MSSPBehaviourMonitor_OnProcessHooked(int intProcessID, string strProcessName)
        {
            string[] processItems = { intProcessID.ToString(), strProcessName };
            var oProcessItem = new ListViewItem(processItems);
            
            lvMonitoredProcesses.Items.Add(oProcessItem);
        }

        public void MSSPBehaviourMonitor_OnFileCreated(DateTime dtNow, int intProcessID, string strFileName)
        {
            txtBehaviourLog.AppendText(string.Format("{0}: Process {1} Opened \"{2}\" for Writing\n", dtNow.ToString(), intProcessID.ToString(), strFileName));
        }

        public void mMonitor_OnProcessHooked(int intProcessID, string strProcessName)
        {
            MessageBox.Show(strProcessName);
        }

        public void mMonitor_OnFileCreated(DateTime dtNow, int intProcessID, string strFileName)
        {
            MessageBox.Show(dtNow.ToShortDateString() + " " + intProcessID + " " + strFileName);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bgScanner.CancelAsync();
            bgScanner.Dispose();
            tickTimer.Stop();

            this.progressBar1.Value = 0;
            this.btnCancel.Enabled = false;
            this.btnScan.Enabled = true;

            mScanner = null;

            MSSPLogger.WriteToLog(LogPath, "Scan Cancelled by User");
        }

        private void mScanner_ProgressUpdateHandler(string strDirectory, string strFile, string strFileCount)
        {
            txtCurrentDir.Text = strDirectory;
            txtCurrentFile.Text = strFile;
            txtItemsScanned.Text = strFileCount;
        }

        private void mScanner_ScanCompleteHandler(string strDirectory, string strDirectoryCount, string strFileCount)
        {
            bgScanner.CancelAsync();
            bgScanner.Dispose();
            tickTimer.Stop();
            mProgramTimer.Stop();

            mScanner = null;

            this.progressBar1.Value = 0;
            this.btnCancel.Enabled = false;
            this.btnScan.Enabled = true;

            txtCurrentDir.Text = "Scan of " + strDirectory + " Complete!";
            txtCurrentFile.Text = "";
            txtItemsScanned.Text = strFileCount + " Total Files Scanned!";

            TimeSpan time = mProgramTimer.Elapsed;

            txtElapsedTime.Text = string.Format("{0}:{1}:{2} {3}", time.Hours <= 0 ? "00" : time.Hours < 10 ? "0" + time.Hours.ToString() : time.Hours.ToString(),
                                                                   time.Minutes <= 0 ? "00" : time.Minutes < 10 ? "0" + time.Minutes.ToString() : time.Minutes.ToString(),
                                                                   time.Seconds <= 0 ? "00" : time.Seconds < 10 ? "0" + time.Seconds.ToString() : time.Seconds.ToString(),
                                                                   "Total Elapsed Time!");

            mProgramTimer.Reset();
        }

        private void mScanner_VirusDetectedHandler(string strDirectory, string strFile, string strVirusName)
        {
            MSSPVirusActionDialog oDialog = new MSSPVirusActionDialog(strDirectory, strFile, strVirusName);

            oDialog.Show();
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
                    MSSPLogger.WriteToLog(LogPath, "Error Populating Tree View. Error: " + ex.Message);
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
                MSSPLogger.WriteToLog(LogPath, "Error Populating Tree View. Error: " + ex.Message);
            }
        }
    }
}
