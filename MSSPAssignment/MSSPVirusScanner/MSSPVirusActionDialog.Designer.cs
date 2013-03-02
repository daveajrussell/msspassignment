namespace MSSPVirusScanner
{
    partial class MSSPVirusActionDialog
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
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnQuarantine = new System.Windows.Forms.Button();
            this.btnDoNothing = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtVirus = new System.Windows.Forms.TextBox();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.txtDirectory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDirectory = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(12, 178);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnQuarantine
            // 
            this.btnQuarantine.Location = new System.Drawing.Point(184, 178);
            this.btnQuarantine.Name = "btnQuarantine";
            this.btnQuarantine.Size = new System.Drawing.Size(75, 23);
            this.btnQuarantine.TabIndex = 0;
            this.btnQuarantine.Text = "Quarantine";
            this.btnQuarantine.UseVisualStyleBackColor = true;
            // 
            // btnDoNothing
            // 
            this.btnDoNothing.Location = new System.Drawing.Point(348, 178);
            this.btnDoNothing.Name = "btnDoNothing";
            this.btnDoNothing.Size = new System.Drawing.Size(75, 23);
            this.btnDoNothing.TabIndex = 0;
            this.btnDoNothing.Text = "Do Nothing";
            this.btnDoNothing.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtVirus);
            this.groupBox1.Controls.Add(this.txtFile);
            this.groupBox1.Controls.Add(this.txtDirectory);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblDirectory);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(411, 160);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "A Virus Has Been Detected!";
            // 
            // txtVirus
            // 
            this.txtVirus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtVirus.Location = new System.Drawing.Point(66, 102);
            this.txtVirus.Multiline = true;
            this.txtVirus.Name = "txtVirus";
            this.txtVirus.ReadOnly = true;
            this.txtVirus.Size = new System.Drawing.Size(339, 35);
            this.txtVirus.TabIndex = 5;
            // 
            // txtFile
            // 
            this.txtFile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFile.Location = new System.Drawing.Point(66, 61);
            this.txtFile.Multiline = true;
            this.txtFile.Name = "txtFile";
            this.txtFile.ReadOnly = true;
            this.txtFile.Size = new System.Drawing.Size(339, 35);
            this.txtFile.TabIndex = 4;
            // 
            // txtDirectory
            // 
            this.txtDirectory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDirectory.Location = new System.Drawing.Point(66, 20);
            this.txtDirectory.Multiline = true;
            this.txtDirectory.Name = "txtDirectory";
            this.txtDirectory.ReadOnly = true;
            this.txtDirectory.Size = new System.Drawing.Size(339, 35);
            this.txtDirectory.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Virus:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "File:";
            // 
            // lblDirectory
            // 
            this.lblDirectory.AutoSize = true;
            this.lblDirectory.Location = new System.Drawing.Point(7, 20);
            this.lblDirectory.Name = "lblDirectory";
            this.lblDirectory.Size = new System.Drawing.Size(52, 13);
            this.lblDirectory.TabIndex = 0;
            this.lblDirectory.Text = "Directory:";
            // 
            // MSSPVirusActionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 213);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDoNothing);
            this.Controls.Add(this.btnQuarantine);
            this.Controls.Add(this.btnDelete);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MSSPVirusActionDialog";
            this.Text = "MSSPVirusActionDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnQuarantine;
        private System.Windows.Forms.Button btnDoNothing;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtVirus;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.TextBox txtDirectory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDirectory;
    }
}