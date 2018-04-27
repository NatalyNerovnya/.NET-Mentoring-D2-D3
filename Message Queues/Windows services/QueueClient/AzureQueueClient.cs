namespace QueueClient
{
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    public class AzureQueueClient
    {
        private string pdfMessageQueueName = "FileQueue";
        private string statusMessageQueueName = "StatusQueue";
        private QueueClient fileQueueClient;
        private QueueClient statusQueueClient;
        private NamespaceManager namespaceManager;

        public AzureQueueClient()
        {
            namespaceManager = NamespaceManager.Create();

            fileQueueClient = CreateQueueClient(pdfMessageQueueName);
            statusQueueClient = CreateQueueClient(statusMessageQueueName);
        }

        public void SendFileBytes(byte[] byteArray)
        {
            SendBytes(fileQueueClient, byteArray);
        }

        public void SendStatusBytes(byte[] byteArray)
        {
            SendBytes(statusQueueClient, byteArray);
        }

        private void SendBytes(QueueClient client, byte[] data)
        {
            var message = new BrokeredMessage(data);
            client.Send(message);
        }

        private QueueClient CreateQueueClient(string queueName)
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
