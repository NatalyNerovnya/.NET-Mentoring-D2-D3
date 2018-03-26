using System;
using System.Collections.Generic;
using System.Configuration;
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
            foreach (var uriString in _uriStringsList)
            {
                var cancelationTokenSource = new CancellationTokenSource();
                var cancelString = $"Cancel {new Uri(uriString).Host}";

                var button = new Button() { Text = cancelString, Width = 300 };
                button.Click += (o, args) => {
                    cancelationTokenSource?.Cancel();
                    RemovePanelsElements(button, uriString);
                };

                this.flowLayoutPanel1.Controls.Add(button);
                var task = this.HabdleDownloadedClickAsync(uriString, cancelationTokenSource);

                this.flowLayoutPanel2.Controls.Remove(flowLayoutPanel2.Controls.Find(uriString, false)[0]);

                task.ContinueWith((t) =>
                {
                    RemovePanelsElements(button, uriString);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
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

        private void RemovePanelsElements(Button button, string uriString)
        {
            this.flowLayoutPanel1.Controls.Remove(button);
            this._uriStringsList.Remove(uriString);
        }
    }
}
