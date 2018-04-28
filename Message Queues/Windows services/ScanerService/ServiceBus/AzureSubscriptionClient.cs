using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using ScanerService.Status;

namespace ScanerService.ServiceBus
{
    public class AzureSubscriptionClient
    {
        private string topicName = "CentralTopic";
        private string subscriptionName = "Subscription1";
        private SubscriptionClient subscriptionClient;
        private NamespaceManager namespaceManager;
        private StatusService statusService;

        public AzureSubscriptionClient(StatusService service)
        {
            statusService = service;
            namespaceManager = NamespaceManager.Create();

            CreateSubscription();
        }

        private void ProcessMessage(BrokeredMessage message)
        {
            var body = message.GetBody<string>();

            if (string.IsNullOrEmpty(body))
            {
                statusService.SendStatus();
            }
        }

        private void CreateSubscription()
        {
            if (!namespaceManager.TopicExists(topicName))
            {
                namespaceManager.CreateTopic(topicName);
            }

            if (!namespaceManager.SubscriptionExists(topicName, subscriptionName))
            {
                namespaceManager.CreateSubscription(topicName, subscriptionName);
            }

            var messagingFactory = MessagingFactory.Create(namespaceManager.Address, namespaceManager.Settings.TokenProvider);

            subscriptionClient = messagingFactory.CreateSubscriptionClient(topicName, subscriptionName);
            subscriptionClient.OnMessage(ProcessMessage);
        }
    }
}
