namespace QueueClient
{
    using DocumentService;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.IO;
    using Topshelf;

    public class AzureQueueClient : ServiceControl
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

        public bool Start(HostControl hostControl)
        {
            ListenQueue(pdfQueueClient, ProcessMessage);
            ListenQueue(statusQueueClient, ProcessStatus);

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            pdfQueueClient.Close();
            statusQueueClient.Close();

            return true;
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
