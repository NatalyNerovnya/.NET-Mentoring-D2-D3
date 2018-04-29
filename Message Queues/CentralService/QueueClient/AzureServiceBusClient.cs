using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading;
using Topshelf;

namespace QueueClient
{
    public class AzureServiceBusClient : ServiceControl
    {
        private AzureQueueClient azureQueueClient;
        private AzureTopicClient azureTopicClient;
        private string watchedFile = "defaultConfiguration.xml";
        private string filePath = @".\..\..\..";
        private FileSystemWatcher watcher;

        public AzureServiceBusClient()
        {
            azureQueueClient = new AzureQueueClient();
            azureTopicClient = new AzureTopicClient();

            watcher = new FileSystemWatcher()
            {
                Path = filePath,
                Filter = watchedFile,
                NotifyFilter = NotifyFilters.LastWrite
            };

            watcher.Changed += Watcher_Changed;
        }

        public bool Start(HostControl hostControl)
        {
            watcher.EnableRaisingEvents = true;

            azureQueueClient.OnStart();
            azureTopicClient.OnStart();

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            watcher.EnableRaisingEvents = false;

            azureQueueClient.OnStop();
            azureTopicClient.OnStop();

            return true;
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            var path = Path.Combine(filePath, watchedFile);

            Thread.Sleep(1000);

            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                azureTopicClient.Send(new Microsoft.ServiceBus.Messaging.BrokeredMessage(fileStream) { ContentType = "StatusConfiguration" });
            }
        }
    }
}
