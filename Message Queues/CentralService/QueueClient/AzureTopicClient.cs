using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Timers;

namespace QueueClient
{
    public class AzureTopicClient
    {
        private string topicName = "CentralTopic";
        private static readonly int timerValue = 70000;
        private TopicClient topicClient;
        private NamespaceManager namespaceManager;
        private Timer timer;

        public AzureTopicClient()
        {
            timer = new Timer(timerValue);
            timer.Elapsed += Timer_Elapsed;
            namespaceManager = NamespaceManager.Create();

            topicClient = CreateTopic(topicName);
        }

        public void Send(BrokeredMessage message)
        {
            topicClient.Send(message);
        }

        public void OnStart()
        {
            timer.Start();
        }

        public void OnStop()
        {
            timer.Stop();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            topicClient.Send(new BrokeredMessage());
        }
        
        private TopicClient CreateTopic(string topicName)
        {
            if (!namespaceManager.TopicExists(topicName))
            {
                namespaceManager.CreateTopic(topicName);
            }

            var factory = MessagingFactory.Create(namespaceManager.Address, namespaceManager.Settings.TokenProvider);

            return factory.CreateTopicClient(topicName);
        }
    }
}