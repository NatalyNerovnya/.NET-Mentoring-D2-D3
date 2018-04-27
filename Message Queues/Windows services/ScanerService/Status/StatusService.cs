using QueueClient;
using System.IO;
using System.Xml.Serialization;

namespace ScanerService.Status
{
    public class StatusService
    {
        private AzureQueueClient queueClient;
        public ServiceStatus ServiceStatus { get; set; }
        
        public StatusService(string barcodeString, int pageTimeout, CurerntState status, AzureQueueClient client)
        {
            queueClient = client;

            ServiceStatus = new ServiceStatus()
            {
                BarcodeString = barcodeString,
                PageTimeout = pageTimeout,
                Status = status
            };
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
    }
}
