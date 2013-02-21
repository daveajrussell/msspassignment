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
            this.scAppContainer = new System.Windows.Forms.SplitContainer();
            this.tvDirectories = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbVirusScan = new System.Windows.Forms.GroupBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.lblText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.scAppContainer)).BeginInit();
            this.scAppContainer.Panel1.SuspendLayout();
            this.scAppContainer.Panel2.SuspendLayout();
            this.scAppContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbVirusScan.SuspendLayout();
            this.SuspendLayout();
            // 
            // scAppContainer
            // 
            this.scAppContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scAppContainer.Location = new System.Drawing.Point(0, 0);
            this.scAppContainer.Name = "scAppContainer";
            // 
            // scAppContainer.Panel1
            // 
            this.scAppContainer.Panel1.Controls.Add(this.tvDirectories);
            // 
            // scAppContainer.Panel2
            // 
            this.scAppContainer.Panel2.Controls.Add(this.panel1);
            this.scAppContainer.Size = new System.Drawing.Size(684, 362);
            this.scAppContainer.SplitterDistance = 228;
            this.scAppContainer.TabIndex = 0;
            // 
            // tvDirectories
            // 
            this.tvDirectories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvDirectories.ImageIndex = 0;
            this.tvDirectories.ImageList = this.imageList1;
            this.tvDirectories.Location = new System.Drawing.Point(0, 0);
            this.tvDirectories.Name = "tvDirectories";
            this.tvDirectories.SelectedImageIndex = 0;
            this.tvDirectories.Size = new System.Drawing.Size(228, 362);
            this.tvDirectories.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gbVirusScan);
            this.panel1.Controls.Add(this.btnScan);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(452, 362);
            this.panel1.TabIndex = 0;
            // 
            // gbVirusScan
            // 
            this.gbVirusScan.Controls.Add(this.lblText);
            this.gbVirusScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbVirusScan.Location = new System.Drawing.Point(0, 0);
            this.gbVirusScan.Name = "gbVirusScan";
            this.gbVirusScan.Size = new System.Drawing.Size(452, 339);
            this.gbVirusScan.TabIndex = 1;
            this.gbVirusScan.TabStop = false;
            this.gbVirusScan.Text = "Virus Scan";
            // 
            // btnScan
            // 
            this.btnScan.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnScan.Location = new System.Drawing.Point(0, 339);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(452, 23);
            this.btnScan.TabIndex = 0;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(7, 20);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(0, 13);
            this.lblText.TabIndex = 0;
            // 
            // MSSPVirusScannerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 362);
            this.Controls.Add(this.scAppContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MSSPVirusScannerForm";
            this.Text = "MSSP Virus Scanner";
            this.scAppContainer.Panel1.ResumeLayout(false);
            this.scAppContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scAppContainer)).EndInit();
            this.scAppContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.gbVirusScan.ResumeLayout(false);
            this.gbVirusScan.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scAppContainer;
        private System.Windows.Forms.TreeView tvDirectories;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.GroupBox gbVirusScan;
        private System.Windows.Forms.Label lblText;

    }
}

