using System;
using Topshelf;

namespace QueueClient
{
    public class AzureServiceBusClient : ServiceControl
    {
        private AzureQueueClient azureQueueClient;
        private AzureTopicClient azureTopicClient;
        public bool Start(HostControl hostControl)
        {
            azureQueueClient = new AzureQueueClient();
            azureTopicClient = new AzureTopicClient();

            azureQueueClient.OnStart();
            azureTopicClient.OnStart();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            azureQueueClient.OnStop();
            azureTopicClient.OnStop();

            return true;
        }
    }
}
