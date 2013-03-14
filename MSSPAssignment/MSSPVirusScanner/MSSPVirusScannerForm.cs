using MSSPVirusScanner.MSSPScanStrategies;
using MSSPVirusScanner.Utils;
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
    /// <summary>
    /// Class listing for the main virus scanning application.
    /// </summary>
    public partial class MSSPVirusScannerForm : Form
    {
        private string LogPath { get; set; }

        private MSSPScanner mScanner;
        private MSSPBehaviourMonitor mMonitor;
        private Stopwatch mProgramTimer;
        private MSSPSignatureAnalysisStrategy mStrategy;
        private MSSPFileSignatureAnalysisStrategy mIdentificationStrategy;
        
        /// <summary>
        /// Class constructor. Initialise the form and all program variables.
        /// </summary>
        public MSSPVirusScannerForm()
        {
            InitializeComponent();
            // Call function to populate the tree view file browser
            PopulateTreeView();

            // Assign event handlers for the mouse click, timer tick and radio button handlers
            this.tvDirectories.NodeMouseClick += new TreeNodeMouseClickEventHandler(this.tvDirectories_NodeMouseClick);
            this.btnCancel.Click += btnCancel_Click;
            this.btnScan.Click += btnScan_Click;
            this.tickTimer.Tick += tickTimer_Tick;

            // Assign a new file signature strategy based on the user selection
            this.rdoScanFull.CheckedChanged += rdoScanFull_CheckedChanged;
            this.rdoScanQuick.CheckedChanged += rdoScanQuick_CheckedChanged;

            // Assign a new string scanning technique based on the selection
            this.rdoBoyerMoore.CheckedChanged += rdoBoyerMoore_CheckedChanged;
            this.rdoStringContains.CheckedChanged += rdoStringContains_CheckedChanged;
            this.rdoStringIndexOf.CheckedChanged += rdoStringIndexOf_CheckedChanged;

            // Assign event handlers to the virus scanner background woker
            this.bgScanner.DoWork += bgScanner_DoWork;
            this.bgScanner.ProgressChanged += bgScanner_ProgressChanged;

            // Allow the background worker to report its progress
            this.bgScanner.WorkerReportsProgress = true;
            this.bgScanner.WorkerSupportsCancellation = true;

            // Assign a handler for the behaviour monitor background woker
            this.bgBehaviourMonitor.DoWork += bgBehaviourMonitor_DoWork;

            this.bgBehaviourMonitor.WorkerSupportsCancellation = true;

            this.mProgramTimer = new Stopwatch();

            // Provide default values for the signature and string scanning strategies
            this.mStrategy = new BoyerMooreAnalysisStrategy();
            this.mIdentificationStrategy = new QuickScanStrategy();

            // Create a new log for this scan
            LogPath = string.Format("C:\\Work\\ScanLogs\\ScanLog_{0}_{1}.txt", DateTime.Now.Ticks, DateTime.Now.ToString("yyyyMMdd"));

            // Start the behaviour monitor running
            StartBehaviourMonitor();
        }

        /// <summary>
        /// Handler for the user checking the String.IndexOf radio button
        /// </summary>
        void rdoStringIndexOf_CheckedChanged(object sender, EventArgs e)
        {
            this.mStrategy = new IndexOfStrategy();
        }

        /// <summary>
        /// Handler for the user checking the String.Contains radio button
        /// </summary>
        void rdoStringContains_CheckedChanged(object sender, EventArgs e)
        {
            this.mStrategy = new ContainsAnalysisStrategy();
        }

        /// <summary>
        /// Handler for the user checking the Boyer Moore scanning radio button
        /// </summary>
        void rdoBoyerMoore_CheckedChanged(object sender, EventArgs e)
        {
            // Reassign the string scanning strategy
            this.mStrategy = new BoyerMooreAnalysisStrategy();
        }

        /// <summary>
        /// Handler for the user checking the Quick Scan radio button
        /// </summary>
        private void rdoScanQuick_CheckedChanged(object sender, EventArgs e)
        {
            // Reassign the identification strategy
            this.mIdentificationStrategy = new QuickScanStrategy();
        }

        /// <summary>
        /// Handler for the user checking the Full Scan radio button
        /// </summary>
        private void rdoScanFull_CheckedChanged(object sender, EventArgs e)
        {
            // Reassign the identification strategy
            this.mIdentificationStrategy = new FullScanStrategy();
        }

        /// <summary>
        /// Event handler called every time the timer 'ticks'
        /// </summary>
        private void tickTimer_Tick(object sender, EventArgs e)
        {
            // Get the elapsed time 
            TimeSpan time = mProgramTimer.Elapsed;
            
            // Create a string from the TimeSpan object indicating how long has passed.
            txtElapsedTime.Text = MSSPUtils.ElapsedTime(time);
        }

        /// <summary>
        /// Event handler for the Scan button click event
        /// </summary>
        private void btnScan_Click(object sender, EventArgs e)
        {
            // Display a message box if the user has not selected a directory
            if (null == tvDirectories.SelectedNode)
            {
                MessageBox.Show("Select a Directory to Scan");
                return;
            }

            // Initiate program timers
            txtStartTime.Text = DateTime.Now.ToShortTimeString();
            tickTimer.Start();
            mProgramTimer.Start();

            // Set UI 
            this.btnCancel.Enabled = true;
            this.btnScan.Enabled = false;
            this.gbAnalysisOptions.Enabled = false;
            this.gbScanOptions.Enabled = false;

            this.txtCurrentDir.Text = "";
            this.txtCurrentFile.Text = "";
            this.txtItemsScanned.Text = "";
            this.txtElapsedTime.Text = "";

            // Initialise scanner variables
            mScanner = new MSSPScanner(LogPath, this, this.mStrategy, this.mIdentificationStrategy);

            // Assign event handlers
            mScanner.ProgressUpdateHandler += mScanner_ProgressUpdateHandler;
            mScanner.ScanCompleteHandler += mScanner_ScanCompleteHandler;
            mScanner.VirusDetectedHandler += mScanner_VirusDetectedHandler;

            // Initiate the scanner thread with the directory to scan
            string strDirectoryToScan = tvDirectories.SelectedNode.FullPath;
            bgScanner.RunWorkerAsync(strDirectoryToScan);
        }

        /// <summary>
        /// Event handler for the Cancel scan button
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Send a cancel async signal to the thread
            bgScanner.CancelAsync();
            bgScanner.Dispose();
            tickTimer.Stop();

            // Reset UI 
            this.progressBar1.Value = 0;
            this.btnCancel.Enabled = false;
            this.btnScan.Enabled = true;

            this.gbAnalysisOptions.Enabled = true;
            this.gbScanOptions.Enabled = true;

            mScanner = null;

            // Tell the log the user cancelled the scan.
            MSSPLogger.WriteToLog(LogPath, "Scan Cancelled by User");
        }

        /// <summary>
        /// Event handler for the background worker thread being invoked
        /// </summary>
        private void bgScanner_DoWork(object sender, DoWorkEventArgs e)
        {
            // Initialise a background worker from the sending argument
            BackgroundWorker oWorker = sender as BackgroundWorker;
            // Call the initiate scan method
            mScanner.InitiateScan(oWorker, e);
        }

        /// <summary>
        /// Event handler triggered when the scanner worker thread reports its progress.
        /// This is used to update the progress bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">The event arguments bubbled up from the thread</param>
        private void bgScanner_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (100 >= e.ProgressPercentage)
                this.progressBar1.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Function called to start the behaviour monitor
        /// </summary>
        private void StartBehaviourMonitor()
        {
            // Initialise the monitor
            mMonitor = new MSSPBehaviourMonitor(LogPath, this);
            
            // Assign event handlers.
            MSSPBehaviourMonitor.OnFileCreated += MSSPBehaviourMonitor_OnFileCreated;
            MSSPBehaviourMonitor.OnProcessHooked += MSSPBehaviourMonitor_OnProcessHooked;

            // Begin the background thread
            this.bgBehaviourMonitor.RunWorkerAsync();
       }

        /// <summary>
        /// DoWork event handler for the background worker thread.
        /// Initiates the behaviour monitor
        /// </summary>
        private void bgBehaviourMonitor_DoWork(object sender, DoWorkEventArgs e)
        {
            mMonitor.InitiateMonitor();
        }

        /// <summary>
        /// Event handler triggered when a process
        /// has been hooked
        /// </summary>
        /// <param name="intProcessID">Id of the hooked process</param>
        /// <param name="strProcessName">Name of the hooked process</param>
        public void MSSPBehaviourMonitor_OnProcessHooked(int intProcessID, string strProcessName)
        {
            string[] processItems = { intProcessID.ToString(), strProcessName };
            var oProcessItem = new ListViewItem(processItems);
            
            lvMonitoredProcesses.Items.Add(oProcessItem);
        }

        /// <summary>
        /// Event handler triggered when a hooked process
        /// creates a file.
        /// </summary>
        /// <param name="dtNow">Timestamp</param>
        /// <param name="intProcessID">ID of the process causing the action</param>
        /// <param name="strFileName">The file name that the process created</param>
        public void MSSPBehaviourMonitor_OnFileCreated(DateTime dtNow, int intProcessID, string strFileName)
        {
            txtBehaviourLog.AppendText(string.Format("{0}: Process {1} Opened \"{2}\" for Writing\n", dtNow.ToString(), intProcessID.ToString(), strFileName));
        }
        
        /// <summary>
        /// The scanner sends progress updates to this method
        /// to show progress in the main application.
        /// </summary>
        /// <param name="strDirectory">Current dir</param>
        /// <param name="strFile">Current file</param>
        /// <param name="strFileCount">Current file count</param>
        private void mScanner_ProgressUpdateHandler(string strDirectory, string strFile, string strFileCount)
        {
            txtCurrentDir.Text = strDirectory;
            txtCurrentFile.Text = strFile;
            txtItemsScanned.Text = strFileCount;
        }

        /// <summary>
        /// Once the scan is complete, this handler is invoked. Allowing
        /// completion information to be shown in the main application.
        /// </summary>
        /// <param name="strDirectory">The initial directory that was scanned</param>
        /// <param name="strDirectoryCount">The number of directories that were scanned</param>
        /// <param name="strFileCount">The number of files that were scanned</param>
        private void mScanner_ScanCompleteHandler(string strDirectory, string strDirectoryCount, string strFileCount)
        {
            // Cancel and dispose the background worker thread
            bgScanner.CancelAsync();
            bgScanner.Dispose();
            
            // Stop the stopwatches
            tickTimer.Stop();
            mProgramTimer.Stop();

            // Dereference variables
            mScanner = null;

            // Reset UI elements
            this.progressBar1.Value = 0;
            this.btnCancel.Enabled = false;
            this.btnScan.Enabled = true;

            this.gbAnalysisOptions.Enabled = true;
            this.gbScanOptions.Enabled = true;

            // Display an indication to the user
            txtCurrentDir.Text = "Scan of " + strDirectory + " Complete!";
            txtCurrentFile.Text = "";
            txtItemsScanned.Text = strFileCount + " Total Files Scanned!";

            TimeSpan time = mProgramTimer.Elapsed;

            txtElapsedTime.Text = MSSPUtils.ElapsedTime(time) + " Total Elapsed Time!";

            mProgramTimer.Reset();
        }

        /// <summary>
        /// In the event of a virus being detected, the event is bubbled up to this
        /// handler. A dialog form is opened with the information and a set of dummy actions
        /// the user may perform,
        /// </summary>
        /// <param name="strDirectory">Directory the virus was detected in</param>
        /// <param name="strFile">File the virus was detected in</param>
        /// <param name="strVirusName">The name of the virus that was detected</param>
        private void mScanner_VirusDetectedHandler(string strDirectory, string strFile, string strVirusName)
        {
            // Create a new dialog form
            MSSPVirusActionDialog oDialog = new MSSPVirusActionDialog(strDirectory, strFile, strVirusName);

            // Open it
            oDialog.Show();
        }

        /// <summary>
        /// Initial function to populate a tree view with directories
        /// that the user may scan.
        /// </summary>
        private void PopulateTreeView()
        {
            // Create a root TreeNode object
            TreeNode tnRoot;

            // Add a folder image icon to the tree view
            tvDirectories.ImageList.Images.Add("Folder", Image.FromFile(Application.StartupPath + @"\Utils\Images\folder.ico"));

            // Get all system drives
            foreach (var drive in DriveInfo.GetDrives())
            {
                // Get the directory info for the current drive
                DirectoryInfo oDirInfo = new DirectoryInfo(drive.Name);

                try
                {
                    if (oDirInfo.Exists)
                    {
                        // Create a new root of this drive
                        tnRoot = new TreeNode(oDirInfo.Name);
                        tnRoot.Tag = oDirInfo;
                        tnRoot.ImageKey = "Folder";
                        // Recurse through child directories
                        GetDirectories(oDirInfo.GetDirectories(), tnRoot);
                        tvDirectories.Nodes.Add(tnRoot);
                    }
                }
                catch (Exception ex)
                {
                    // Write any errors to the log
                    MSSPLogger.WriteToLog(LogPath, "Error Populating Tree View. Error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Recursive function to populate the TreeView with all Top Level 
        /// directories. We only want to show the top level directories as
        /// recursing through all directories is inefficient and slow.
        /// </summary>
        /// <param name="arrSubDirs">An array of subdirectory info objects</param>
        /// <param name="oParentNode">The root node</param>
        private void GetDirectories(DirectoryInfo[] arrSubDirs, TreeNode oParentNode)
        {
            TreeNode oNode;
            DirectoryInfo[] subSubDirs;

            // Iterate through each sub directory for the given array of sub directories
            foreach (DirectoryInfo subDir in arrSubDirs)
            {
                // Create a Tree node object
                oNode = new TreeNode(subDir.Name, 0, 0);
                oNode.Nodes.Add("");
                oNode.Tag = subDir;
                oNode.ImageKey = "Folder";

                // Get all top level subdirectories belonging to this directory
                subSubDirs = subDir.GetDirectories("", SearchOption.TopDirectoryOnly);

                // Call the recursive function to populate this TreeNode with its
                // child nodes.
                if (0 != subSubDirs.Length)
                    GetDirectories(subSubDirs, oNode);

                // Add the treenode to the root
                oParentNode.Nodes.Add(oNode);
            }
        }

        /// <summary>
        /// Function to populate the treeview with the directories
        /// belonging to the directory that was selected.
        /// This is to ensure best efficiency as populating the
        /// entire tree view on start up is very slow. We only want to show
        /// directories the user has clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">The tree node object that was clicked by the user</param>
        private void tvDirectories_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Get the Tree Node from the event object
            TreeNode tnSelectedNode = e.Node;
            // Clear the blank node that was added to force the expansion icon to appear
            e.Node.Nodes.Clear();

            try
            {
                // Populate the TreeView with the directories belonging to the clicked directory
                GetDirectories(new DirectoryInfo(tnSelectedNode.FullPath).GetDirectories(), tnSelectedNode);
            }
            catch (Exception ex)
            {
                // Log any error that arises from trying to populate the TreeView
                MSSPLogger.WriteToLog(LogPath, "Error Populating Tree View. Error: " + ex.Message);
            }
        }
    }
}
