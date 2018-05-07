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
        private long currentMessageNumber;



        public AzureQueueClient()
        {
            pdfService = new PdfService();
            xmlService = new XmlService();

            namespaceManager = NamespaceManager.Create();

            pdfQueueClient = CreateQueue(pdfMessageQueueName, true);
            statusQueueClient = CreateQueue(statusQueueName, false);
        }


        public MemoryStream Receive()
        {
            var largeMessageStream = new MemoryStream();
            var session = pdfQueueClient.AcceptMessageSession();
            var numberOfSubMessages = -1;
            var numberOfSubMessagesReceived = 0;

            while (true)
            {
                var subMessage = session.Receive();

                if (subMessage != null)
                {
                    if (numberOfSubMessages == -1)
                    {
                        numberOfSubMessages = (int)subMessage.Properties["NumberOfSubMessages"];
                    }

                    var subMessageStream = subMessage.GetBody<Stream>();

                    subMessageStream.CopyTo(largeMessageStream);
                    subMessage.Complete();
                    
                    numberOfSubMessagesReceived++;

                    if (numberOfSubMessagesReceived == numberOfSubMessages)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            largeMessageStream.Seek(0, SeekOrigin.Begin);
            return largeMessageStream;
        }

        public void OnStart()
        {
            ListenQueue(statusQueueClient, ProcessStatus);

            while (true)
            {
                var stream = Receive();
               pdfService.SaveDocument(stream);
            }
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

        private void ProcessStatus(BrokeredMessage message)
        {
            xmlService.SaveDocument(new MemoryStream(message.GetBody<byte[]>()));
        }
        
        private QueueClient CreateQueue(string queueName, bool requierSession)
        {
            if (!namespaceManager.QueueExists(queueName))
            {
                var q = new QueueDescription(queueName);
                q.RequiresSession = requierSession;
                q.EnablePartitioning = requierSession;
                namespaceManager.CreateQueue(queueName);
            }

            var messagingFactory = MessagingFactory.Create(
                namespaceManager.Address,
                namespaceManager.Settings.TokenProvider);

            return messagingFactory.CreateQueueClient(queueName);
        }
    }
}
