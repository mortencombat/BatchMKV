using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BatchMKV
{
    public partial class fAbout : Form
    {
        private readonly int yearStart = 2015;

        public fAbout()
        {
            InitializeComponent();

            Version version;
            try { version = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion; }
            catch { version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version; }
            int yearCurrent = DateTime.Now.Year;
            this.lblAbout.Text = String.Format("BatchMKV\r\nVersion {0}\r\n© {1} Adeptweb.\r\nAll rights reserved.",
                version.ToString(4),
                String.Format(yearCurrent > yearStart ? "{0} - {1}" : "{0}", yearStart, yearCurrent));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblSupport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openURL("http://www.adeptweb.dk/batchmkv");
        }

        private void lblMakeMKV_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openURL("http://www.makemkv.com/");
        }

        private void lblMKVToolNix_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openURL("https://www.bunkus.org/videotools/mkvtoolnix/");
        }

        private void openURL(string url)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = url;
            p.Start();
        }

    }
}
