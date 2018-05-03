namespace QueueClient
{
    using DocumentService;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.IO;
    public class AzureQueueClient
    {
        private string pdfMessageQueueName = "FileQueue";
        private string statusQueueName = "StatusQueue";
        private QueueClient pdfQueueClient;
        private QueueClient statusQueueClient;
        private NamespaceManager namespaceManager;
        private PdfService pdfService;
        private XmlService xmlService;
        private MemoryStream largeMessageStream;
        private long currentMessageNumber;



        public AzureQueueClient()
        {
            pdfService = new PdfService();
            xmlService = new XmlService();

            namespaceManager = NamespaceManager.Create();

            pdfQueueClient = CreateQueue(pdfMessageQueueName);
            statusQueueClient = CreateQueue(statusQueueName);

            largeMessageStream = new MemoryStream();
        }

        public void OnStart()
        {
            ListenQueue(pdfQueueClient, ProcessMessage);
            ListenQueue(statusQueueClient, ProcessStatus);
        }

        public void OnStop()
        {
            pdfQueueClient.Close();
            statusQueueClient.Close();
        }

        private void ListenQueue(QueueClient client, Action<BrokeredMessage> action)
        {
            client.OnMessage(action);
        }

        private void ProcessMessage(BrokeredMessage message)
        {
            if (message.Properties.ContainsKey("NumberOfSubMessages"))
            {
                ProcessBatchPart(message);
            }
            else
            {
                pdfService.SaveDocument(message.GetBody<Stream>());
            }
        }

        private void ProcessStatus(BrokeredMessage message)
        {
            xmlService.SaveDocument(new MemoryStream(message.GetBody<byte[]>()));
        }

        private void ProcessBatchPart(BrokeredMessage message)
        {
            var subMessageStream = message.GetBody<Stream>();

            subMessageStream.CopyTo(largeMessageStream);
            message.Complete();

            currentMessageNumber = (int)message.Properties["SubMessageNumber"];

            if (currentMessageNumber == (int)message.Properties["NumberOfSubMessages"])
            {
                pdfService.SaveDocument(largeMessageStream);
                largeMessageStream = new MemoryStream();
                currentMessageNumber = 0;
            }
        }
        
        private QueueClient CreateQueue(string queueName)
        {
            if (!namespaceManager.QueueExists(queueName))
            {
                var q = new QueueDescription(queueName);
                q.EnablePartitioning = true;
                q.EnableBatchedOperations = true;
                namespaceManager.CreateQueue(queueName);
            }

            var messagingFactory = MessagingFactory.Create(
                namespaceManager.Address,
                namespaceManager.Settings.TokenProvider);

            return messagingFactory.CreateQueueClient(queueName);
        }
    }
}
