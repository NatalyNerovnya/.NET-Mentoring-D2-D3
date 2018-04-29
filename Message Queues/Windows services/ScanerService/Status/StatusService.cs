using QueueClient;
using System.IO;
using System.Timers;
using System.Xml.Serialization;

namespace ScanerService.Status
{
    public class StatusService
    {
        private AzureQueueClient queueClient;
        private Timer timer;
        public ServiceStatus ServiceStatus { get; set; }

        public StatusService(string barcodeString, int pageTimeout, CurerntState status, AzureQueueClient client)
        {
            queueClient = client;

            timer = new Timer(pageTimeout);
            timer.Elapsed += StatusTimer_Elapsed;

            ServiceStatus = new ServiceStatus()
            {
                BarcodeString = barcodeString,
                PageTimeout = pageTimeout,
                Status = status
            };
        }

        public void OnStart()
        {
            timer.Start();
        }

        public void OnStop()
        {
            timer.Stop();
        }

        public void UpdateTimer(int timerValue)
        {
            OnStop();

            timer.Interval = timerValue;
            ServiceStatus.PageTimeout = timerValue;
            OnStart();
        }

        public void SendStatus()
        {
            var serializer = new XmlSerializer(typeof(ServiceStatus));

            MemoryStream stream;
            using (stream = new MemoryStream())
            {
                serializer.Serialize(stream, ServiceStatus);
            }

            queueClient.SendStatusBytes(stream.ToArray());
        }

        private void StatusTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SendStatus();
        }
    }
}
