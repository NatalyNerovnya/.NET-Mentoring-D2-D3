using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DownloadManager
{
    public partial class Form1 : Form
    {
        private readonly List<string> _uriStringsList;

        public IDownloadWorker Downloader { get; set; }

        public Form1(IDownloadWorker worker)
        {
            InitializeComponent();
            Downloader = worker;

            this.label1.Text = $"Download files you can fined at {ConfigurationManager.AppSettings["FilePath"]}";
            this.label2.Text = string.Empty;

            _uriStringsList = new List<string>();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var uriString = this.input1.Text;
            this._uriStringsList.Add(uriString);
            this.flowLayoutPanel2.Controls.Add(new Label() {Text = uriString, Name = uriString, Width = 300 });
            this.input1.Text = string.Empty;
        }

        private void download1_Click(object sender, EventArgs e)
        {
            var task = this.HabdleDownloadedClickAsync();
        }

        private async Task HabdleDownloadedClickAsync()
        {
            var tasksToDownload = this._uriStringsList.Select(str =>
            {
                var ct = new CancellationTokenSource();
                var cancelString = $"Cancel {new Uri(str).Host}";
                var button = new Button() { Text = cancelString, Width = 300, Name = str };
                button.Click += (o, args) =>
                {
                    ct?.Cancel();
                    RemovePanelsElements(button, str);
                };
                this.flowLayoutPanel1.Controls.Add(button);
                this.flowLayoutPanel2.Controls.Remove(flowLayoutPanel2.Controls.Find(str, false)[0]);
                return DownloadPageAsync(str, ct);
            });

            var downloadTasks = tasksToDownload.ToList();

            while (downloadTasks.Any())
            {
                var finishedTask = await Task.WhenAny(downloadTasks);
                downloadTasks.Remove(finishedTask);
                var uriString = await finishedTask;
                var button = this.flowLayoutPanel1.Controls.Find(uriString, false)[0] as Button;
                RemovePanelsElements(button, uriString);
            }
        }

        private async Task<string> DownloadPageAsync(string uriString, CancellationTokenSource cts)
        {
            try
            {
                await this.Downloader.DownloadPageAsync(uriString, cts.Token);
            }
            catch (OperationCanceledException)
            {
                this.label2.Text += $"\nCanceled: {uriString}";
                cts.Dispose();
                return uriString;
            }
            catch (Exception)
            {
                this.label2.Text += $"\nFailed to download: {uriString}";
                return uriString;
            }

            this.label2.Text += $"\nDownloaded: {uriString}";
            return uriString;
        }

        private void RemovePanelsElements(Button button, string uriString)
        {
            this.flowLayoutPanel1.Controls.Remove(button);
            this._uriStringsList.Remove(uriString);
        }
    }
}
