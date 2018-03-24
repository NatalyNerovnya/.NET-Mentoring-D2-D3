using System;
using System.Windows.Forms;

namespace DownloadManager
{
    public partial class Form1 : Form
    {
        public IDownloadWorker Downloader { get; set; }
        public Form1(IDownloadWorker worker)
        {
            InitializeComponent();
            Downloader = worker;
        }

        private void download1_Click(object sender, EventArgs e)
        {
            var uriString = this.input1.Text;
            Downloader.DownloadPageAsync(uriString);
        }
        
    }
}
