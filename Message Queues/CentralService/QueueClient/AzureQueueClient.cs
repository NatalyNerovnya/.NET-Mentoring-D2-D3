namespace QueueClient
{
    using DocumentService;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using System.IO;
    using Topshelf;

    public class AzureQueueClient : ServiceControl
    {
        private string pdfMessageQueueName = "FileQueue";
        private QueueClient queueClient;
        private NamespaceManager namespaceManager;
        private PdfService pdfService;

        public AzureQueueClient()
        {
            pdfService = new PdfService();
            namespaceManager = NamespaceManager.Create();
            CreateQueue();
        }

        public void SendBytes(byte[] data)
        {
            var message = new BrokeredMessage(data);
            queueClient.Send(message);
        }

        public bool Start(HostControl hostControl)
        {
            ListenQueue();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            queueClient.Close();

            return true;
        }

        private void ListenQueue()
        {
            queueClient.OnMessage(ProcessMessage);
        }

        private void ProcessMessage(BrokeredMessage message)
        {
            pdfService.SaveDocument(message.GetBody<Stream>());
        }

        private void CreateQueue()
        {
            if (!namespaceManager.QueueExists(pdfMessageQueueName))
            {
                namespaceManager.CreateQueue(pdfMessageQueueName);
            }

            var messagingFactory = MessagingFactory.Create(
                namespaceManager.Address,
                namespaceManager.Settings.TokenProvider);

            queueClient = messagingFactory.CreateQueueClient(pdfMessageQueueName);
        }
    }
}
