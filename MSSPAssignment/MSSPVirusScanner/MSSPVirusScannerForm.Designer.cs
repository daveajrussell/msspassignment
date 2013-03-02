namespace MSSPVirusScanner
{
    partial class MSSPVirusScannerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tickTimer = new System.Windows.Forms.Timer(this.components);
            this.bgScanner = new System.ComponentModel.BackgroundWorker();
            this.tcVirusApplication = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.scAppContainer = new System.Windows.Forms.SplitContainer();
            this.tvDirectories = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbVirusScan = new System.Windows.Forms.GroupBox();
            this.txtItemsScanned = new System.Windows.Forms.Label();
            this.txtElapsedTime = new System.Windows.Forms.Label();
            this.txtStartTime = new System.Windows.Forms.Label();
            this.txtCurrentFile = new System.Windows.Forms.TextBox();
            this.txtCurrentDir = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.lblElapsedTime = new System.Windows.Forms.Label();
            this.lblItemsScanned = new System.Windows.Forms.Label();
            this.lblScanned = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.lblElapsed = new System.Windows.Forms.Label();
            this.lblCurrentFile = new System.Windows.Forms.Label();
            this.lblCurrentDir = new System.Windows.Forms.Label();
            this.lblFile = new System.Windows.Forms.Label();
            this.lblDir = new System.Windows.Forms.Label();
            this.lblText = new System.Windows.Forms.Label();
            this.btnScan = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblProcessLog = new System.Windows.Forms.Label();
            this.lblMonitoredProcess = new System.Windows.Forms.Label();
            this.txtBehaviourLog = new System.Windows.Forms.TextBox();
            this.lvMonitoredProcesses = new System.Windows.Forms.ListView();
            this.pidHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tcVirusApplication.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scAppContainer)).BeginInit();
            this.scAppContainer.Panel1.SuspendLayout();
            this.scAppContainer.Panel2.SuspendLayout();
            this.scAppContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbVirusScan.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tcVirusApplication
            // 
            this.tcVirusApplication.Controls.Add(this.tabPage1);
            this.tcVirusApplication.Controls.Add(this.tabPage2);
            this.tcVirusApplication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcVirusApplication.Location = new System.Drawing.Point(0, 0);
            this.tcVirusApplication.Name = "tcVirusApplication";
            this.tcVirusApplication.SelectedIndex = 0;
            this.tcVirusApplication.Size = new System.Drawing.Size(692, 547);
            this.tcVirusApplication.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.scAppContainer);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(684, 521);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Virus Scan";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // scAppContainer
            // 
            this.scAppContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scAppContainer.Location = new System.Drawing.Point(3, 3);
            this.scAppContainer.Name = "scAppContainer";
            // 
            // scAppContainer.Panel1
            // 
            this.scAppContainer.Panel1.Controls.Add(this.tvDirectories);
            // 
            // scAppContainer.Panel2
            // 
            this.scAppContainer.Panel2.Controls.Add(this.panel1);
            this.scAppContainer.Size = new System.Drawing.Size(678, 515);
            this.scAppContainer.SplitterDistance = 226;
            this.scAppContainer.TabIndex = 1;
            // 
            // tvDirectories
            // 
            this.tvDirectories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvDirectories.ImageIndex = 0;
            this.tvDirectories.ImageList = this.imageList1;
            this.tvDirectories.Location = new System.Drawing.Point(0, 0);
            this.tvDirectories.Name = "tvDirectories";
            this.tvDirectories.SelectedImageIndex = 0;
            this.tvDirectories.Size = new System.Drawing.Size(226, 515);
            this.tvDirectories.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbVirusScan);
            this.panel1.Controls.Add(this.btnScan);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(448, 515);
            this.panel1.TabIndex = 0;
            // 
            // gbVirusScan
            // 
            this.gbVirusScan.Controls.Add(this.txtItemsScanned);
            this.gbVirusScan.Controls.Add(this.txtElapsedTime);
            this.gbVirusScan.Controls.Add(this.txtStartTime);
            this.gbVirusScan.Controls.Add(this.txtCurrentFile);
            this.gbVirusScan.Controls.Add(this.txtCurrentDir);
            this.gbVirusScan.Controls.Add(this.btnCancel);
            this.gbVirusScan.Controls.Add(this.progressBar1);
            this.gbVirusScan.Controls.Add(this.lblStartTime);
            this.gbVirusScan.Controls.Add(this.lblElapsedTime);
            this.gbVirusScan.Controls.Add(this.lblItemsScanned);
            this.gbVirusScan.Controls.Add(this.lblScanned);
            this.gbVirusScan.Controls.Add(this.lblStart);
            this.gbVirusScan.Controls.Add(this.lblElapsed);
            this.gbVirusScan.Controls.Add(this.lblCurrentFile);
            this.gbVirusScan.Controls.Add(this.lblCurrentDir);
            this.gbVirusScan.Controls.Add(this.lblFile);
            this.gbVirusScan.Controls.Add(this.lblDir);
            this.gbVirusScan.Controls.Add(this.lblText);
            this.gbVirusScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbVirusScan.Location = new System.Drawing.Point(0, 0);
            this.gbVirusScan.Name = "gbVirusScan";
            this.gbVirusScan.Size = new System.Drawing.Size(448, 492);
            this.gbVirusScan.TabIndex = 1;
            this.gbVirusScan.TabStop = false;
            this.gbVirusScan.Text = "Virus Scan";
            // 
            // txtItemsScanned
            // 
            this.txtItemsScanned.AutoSize = true;
            this.txtItemsScanned.Location = new System.Drawing.Point(108, 103);
            this.txtItemsScanned.Name = "txtItemsScanned";
            this.txtItemsScanned.Size = new System.Drawing.Size(0, 13);
            this.txtItemsScanned.TabIndex = 20;
            // 
            // txtElapsedTime
            // 
            this.txtElapsedTime.AutoSize = true;
            this.txtElapsedTime.Location = new System.Drawing.Point(108, 65);
            this.txtElapsedTime.Name = "txtElapsedTime";
            this.txtElapsedTime.Size = new System.Drawing.Size(0, 13);
            this.txtElapsedTime.TabIndex = 19;
            // 
            // txtStartTime
            // 
            this.txtStartTime.AutoSize = true;
            this.txtStartTime.Location = new System.Drawing.Point(108, 28);
            this.txtStartTime.Name = "txtStartTime";
            this.txtStartTime.Size = new System.Drawing.Size(0, 13);
            this.txtStartTime.TabIndex = 18;
            // 
            // txtCurrentFile
            // 
            this.txtCurrentFile.Location = new System.Drawing.Point(108, 295);
            this.txtCurrentFile.Multiline = true;
            this.txtCurrentFile.Name = "txtCurrentFile";
            this.txtCurrentFile.ReadOnly = true;
            this.txtCurrentFile.Size = new System.Drawing.Size(327, 133);
            this.txtCurrentFile.TabIndex = 17;
            // 
            // txtCurrentDir
            // 
            this.txtCurrentDir.Location = new System.Drawing.Point(108, 139);
            this.txtCurrentDir.Multiline = true;
            this.txtCurrentDir.Name = "txtCurrentDir";
            this.txtCurrentDir.ReadOnly = true;
            this.txtCurrentDir.Size = new System.Drawing.Size(327, 133);
            this.txtCurrentDir.TabIndex = 16;
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(10, 463);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(175, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(10, 434);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(425, 23);
            this.progressBar1.TabIndex = 11;
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Location = new System.Drawing.Point(109, 28);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(0, 13);
            this.lblStartTime.TabIndex = 10;
            // 
            // lblElapsedTime
            // 
            this.lblElapsedTime.AutoSize = true;
            this.lblElapsedTime.Location = new System.Drawing.Point(109, 53);
            this.lblElapsedTime.Name = "lblElapsedTime";
            this.lblElapsedTime.Size = new System.Drawing.Size(0, 13);
            this.lblElapsedTime.TabIndex = 9;
            // 
            // lblItemsScanned
            // 
            this.lblItemsScanned.AutoSize = true;
            this.lblItemsScanned.Location = new System.Drawing.Point(109, 79);
            this.lblItemsScanned.Name = "lblItemsScanned";
            this.lblItemsScanned.Size = new System.Drawing.Size(0, 13);
            this.lblItemsScanned.TabIndex = 8;
            // 
            // lblScanned
            // 
            this.lblScanned.AutoSize = true;
            this.lblScanned.Location = new System.Drawing.Point(7, 104);
            this.lblScanned.Name = "lblScanned";
            this.lblScanned.Size = new System.Drawing.Size(81, 13);
            this.lblScanned.TabIndex = 7;
            this.lblScanned.Text = "Items Scanned:";
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(7, 28);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(58, 13);
            this.lblStart.TabIndex = 6;
            this.lblStart.Text = "Start Time:";
            // 
            // lblElapsed
            // 
            this.lblElapsed.AutoSize = true;
            this.lblElapsed.Location = new System.Drawing.Point(7, 66);
            this.lblElapsed.Name = "lblElapsed";
            this.lblElapsed.Size = new System.Drawing.Size(74, 13);
            this.lblElapsed.TabIndex = 5;
            this.lblElapsed.Text = "Elapsed Time:";
            // 
            // lblCurrentFile
            // 
            this.lblCurrentFile.AutoSize = true;
            this.lblCurrentFile.Location = new System.Drawing.Point(109, 130);
            this.lblCurrentFile.Name = "lblCurrentFile";
            this.lblCurrentFile.Size = new System.Drawing.Size(0, 13);
            this.lblCurrentFile.TabIndex = 4;
            // 
            // lblCurrentDir
            // 
            this.lblCurrentDir.AutoSize = true;
            this.lblCurrentDir.Location = new System.Drawing.Point(109, 105);
            this.lblCurrentDir.Name = "lblCurrentDir";
            this.lblCurrentDir.Size = new System.Drawing.Size(0, 13);
            this.lblCurrentDir.TabIndex = 3;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(7, 298);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(63, 13);
            this.lblFile.TabIndex = 2;
            this.lblFile.Text = "Current File:";
            // 
            // lblDir
            // 
            this.lblDir.AutoSize = true;
            this.lblDir.Location = new System.Drawing.Point(7, 142);
            this.lblDir.Name = "lblDir";
            this.lblDir.Size = new System.Drawing.Size(89, 13);
            this.lblDir.TabIndex = 1;
            this.lblDir.Text = "Current Directory:";
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(7, 20);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(0, 13);
            this.lblText.TabIndex = 0;
            // 
            // btnScan
            // 
            this.btnScan.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnScan.Location = new System.Drawing.Point(0, 492);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(448, 23);
            this.btnScan.TabIndex = 0;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(684, 521);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Behaviour Monitor";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblProcessLog);
            this.groupBox1.Controls.Add(this.lblMonitoredProcess);
            this.groupBox1.Controls.Add(this.txtBehaviourLog);
            this.groupBox1.Controls.Add(this.lvMonitoredProcesses);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(668, 507);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Behaviour Monitor";
            // 
            // lblProcessLog
            // 
            this.lblProcessLog.AutoSize = true;
            this.lblProcessLog.Location = new System.Drawing.Point(9, 266);
            this.lblProcessLog.Name = "lblProcessLog";
            this.lblProcessLog.Size = new System.Drawing.Size(103, 13);
            this.lblProcessLog.TabIndex = 5;
            this.lblProcessLog.Text = "Process Activity Log";
            // 
            // lblMonitoredProcess
            // 
            this.lblMonitoredProcess.AutoSize = true;
            this.lblMonitoredProcess.Location = new System.Drawing.Point(6, 16);
            this.lblMonitoredProcess.Name = "lblMonitoredProcess";
            this.lblMonitoredProcess.Size = new System.Drawing.Size(106, 13);
            this.lblMonitoredProcess.TabIndex = 4;
            this.lblMonitoredProcess.Text = "Monitored Processes";
            // 
            // txtBehaviourLog
            // 
            this.txtBehaviourLog.Location = new System.Drawing.Point(9, 282);
            this.txtBehaviourLog.Multiline = true;
            this.txtBehaviourLog.Name = "txtBehaviourLog";
            this.txtBehaviourLog.ReadOnly = true;
            this.txtBehaviourLog.Size = new System.Drawing.Size(655, 219);
            this.txtBehaviourLog.TabIndex = 2;
            // 
            // lvMonitoredProcesses
            // 
            this.lvMonitoredProcesses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.pidHeader,
            this.pName});
            this.lvMonitoredProcesses.GridLines = true;
            this.lvMonitoredProcesses.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvMonitoredProcesses.Location = new System.Drawing.Point(7, 32);
            this.lvMonitoredProcesses.Name = "lvMonitoredProcesses";
            this.lvMonitoredProcesses.Size = new System.Drawing.Size(655, 231);
            this.lvMonitoredProcesses.TabIndex = 1;
            this.lvMonitoredProcesses.UseCompatibleStateImageBehavior = false;
            this.lvMonitoredProcesses.View = System.Windows.Forms.View.Details;
            // 
            // pidHeader
            // 
            this.pidHeader.Text = "Process ID";
            this.pidHeader.Width = 324;
            // 
            // pName
            // 
            this.pName.Text = "Process Name";
            this.pName.Width = 327;
            // 
            // MSSPVirusScannerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 547);
            this.Controls.Add(this.tcVirusApplication);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MSSPVirusScannerForm";
            this.Text = "MSSP Virus Scanner";
            this.tcVirusApplication.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.scAppContainer.Panel1.ResumeLayout(false);
            this.scAppContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scAppContainer)).EndInit();
            this.scAppContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.gbVirusScan.ResumeLayout(false);
            this.gbVirusScan.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer tickTimer;
        private System.ComponentModel.BackgroundWorker bgScanner;
        private System.Windows.Forms.TabControl tcVirusApplication;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer scAppContainer;
        private System.Windows.Forms.TreeView tvDirectories;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gbVirusScan;
        private System.Windows.Forms.Label txtItemsScanned;
        private System.Windows.Forms.Label txtElapsedTime;
        private System.Windows.Forms.Label txtStartTime;
        private System.Windows.Forms.TextBox txtCurrentFile;
        private System.Windows.Forms.TextBox txtCurrentDir;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.Label lblElapsedTime;
        private System.Windows.Forms.Label lblItemsScanned;
        private System.Windows.Forms.Label lblScanned;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Label lblElapsed;
        private System.Windows.Forms.Label lblCurrentFile;
        private System.Windows.Forms.Label lblCurrentDir;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label lblDir;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtBehaviourLog;
        private System.Windows.Forms.ListView lvMonitoredProcesses;
        private System.Windows.Forms.ColumnHeader pidHeader;
        private System.Windows.Forms.ColumnHeader pName;
        private System.Windows.Forms.Label lblProcessLog;
        private System.Windows.Forms.Label lblMonitoredProcess;

    }
}

