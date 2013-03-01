using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MSSPVirusScanner
{
    public partial class MSSPVirusActionDialog : Form
    {
        public string Directory { get; set; }
        public string File { get; set; }
        public string Virus { get; set; }

        public MSSPVirusActionDialog()
        {
            InitializeComponent();

            this.txtDirectory.Text = Directory;
            this.txtFile.Text = File;
            this.txtVirus.Text = Virus;

            this.btnDelete.Click += btnDelete_Click;
            this.btnQuarantine.Click += btnQuarantine_Click;
            this.btnDoNothing.Click += btnDoNothing_Click;
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnQuarantine_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnDoNothing_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
