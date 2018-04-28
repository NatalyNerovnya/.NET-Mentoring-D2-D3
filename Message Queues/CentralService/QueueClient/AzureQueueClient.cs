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

        public AzureQueueClient()
        {
            pdfService = new PdfService();
            xmlService = new XmlService();

            namespaceManager = NamespaceManager.Create();

            pdfQueueClient = CreateQueue(pdfMessageQueueName);
            statusQueueClient = CreateQueue(statusQueueName);
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
            pdfService.SaveDocument(message.GetBody<Stream>());
        }

        private void ProcessStatus(BrokeredMessage message)
        {
            xmlService.SaveDocument(message.GetBody<Stream>());
        }

        private QueueClient CreateQueue(string queueName)
        {
            if (!namespaceManager.QueueExists(queueName))
            {
                namespaceManager.CreateQueue(queueName);
            }

            var messagingFactory = MessagingFactory.Create(
                namespaceManager.Address,
                namespaceManager.Settings.TokenProvider);

            return messagingFactory.CreateQueueClient(queueName);
        }
    }
}
