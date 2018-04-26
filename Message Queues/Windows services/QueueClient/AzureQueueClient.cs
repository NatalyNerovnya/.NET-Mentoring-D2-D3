namespace QueueClient
{
    //using Microsoft.Azure.NotificationHubs;
    //using Microsoft.Azure.ServiceBus;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    public class AzureQueueClient
    {
        //const string ServiceBusConnectionString = "Endpoint=sb://net-mentoring.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=O1O323CxZpkhx7SYesuje0N7UG5onEaHeyUcgbVOJpY=";
        private string queueName = "FileQueue";
        private QueueClient queueClient;
        private NamespaceManager namespaceManager;

        public AzureQueueClient()
        {
            namespaceManager = NamespaceManager.Create();
            CreateQueue();
        }

        public void SendMessage()
        {
            var message = new BrokeredMessage("some message");

            // Submit the order.
            queueClient.Send(message);
        }

        private void CreateQueue()
        {
            if (!namespaceManager.QueueExists(queueName))
            {
                namespaceManager.CreateQueue(queueName);
            }

            var messagingFactory = MessagingFactory.Create(
                namespaceManager.Address,
                namespaceManager.Settings.TokenProvider);

            queueClient = messagingFactory.CreateQueueClient(queueName);
        }
    }
}
