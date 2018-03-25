using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadManager
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource _cancellationTokenSource1;
        private CancellationTokenSource _cancellationTokenSource2;

        public IDownloadWorker Downloader { get; set; }

        public Form1(IDownloadWorker worker)
        {
            InitializeComponent();
            Downloader = worker;

            this.label1.Text = $"Download files you can fined at {ConfigurationManager.AppSettings["FilePath"]}";
            this.label2.Text = String.Empty;
        }

        private void download1_Click(object sender, EventArgs e)
        {
            var uriString = this.input1.Text;
            _cancellationTokenSource1 = new CancellationTokenSource();

            var task = this.HabdleDownloadedClickAsync(uriString, _cancellationTokenSource1);
            task.ContinueWith((t) =>
            {
                this.input1.Text = string.Empty;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void download2_Click(object sender, EventArgs e)
        {
            var uriString = this.input2.Text;
            _cancellationTokenSource2 = new CancellationTokenSource();

            var task = this.HabdleDownloadedClickAsync(uriString, _cancellationTokenSource2);
            task.ContinueWith((t) =>
            {
                this.input2.Text = string.Empty;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void cancel1_Click(object sender, EventArgs e)
        {
            this._cancellationTokenSource1?.Cancel();
        }

        private void cancel2_Click(object sender, EventArgs e)
        {
            this._cancellationTokenSource2?.Cancel();
        }

        private async Task HabdleDownloadedClickAsync(string uriString, CancellationTokenSource cts)
        {
            try
            {
                await this.Downloader.DownloadPageAsync(uriString, cts.Token);
            }
            catch (OperationCanceledException)
            {
                this.label2.Text += $"\nCanceled: {uriString}";
                cts.Dispose();
                return;
            }
            catch (Exception)
            {
                this.label2.Text += $"\nFailed to download: {uriString}";
                return;
            }
            
            this.label2.Text += $"\nDownloaded: {uriString}";
        }
    }
}
