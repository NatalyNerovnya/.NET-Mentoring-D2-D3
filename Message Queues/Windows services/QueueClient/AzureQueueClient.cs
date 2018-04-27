namespace QueueClient
{
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    public class AzureQueueClient
    {
        private string pdfMessageQueueName = "FileQueue";
        private QueueClient queueClient;
        private NamespaceManager namespaceManager;

        public AzureQueueClient()
        {
            namespaceManager = NamespaceManager.Create();
            CreateQueue();
        }

        public void SendBytes(byte[] data)
        {
            var message = new BrokeredMessage(data);
            queueClient.Send(message);
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
