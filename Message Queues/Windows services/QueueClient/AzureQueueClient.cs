namespace QueueClient
{
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.IO;
    public class AzureQueueClient
    {
        private string pdfMessageQueueName = "FileQueue";
        private string statusMessageQueueName = "StatusQueue";
        private QueueClient fileQueueClient;
        private QueueClient statusQueueClient;
        private NamespaceManager namespaceManager;
        private int MaxMessageSize = 192000;

        public AzureQueueClient()
        {
            namespaceManager = NamespaceManager.Create();

            fileQueueClient = CreateQueueClient(pdfMessageQueueName);
            statusQueueClient = CreateQueueClient(statusMessageQueueName);
        }

        public void SendFileBytes(byte[] byteArray)
        {
            if (byteArray.Length > MaxMessageSize)
                SplitAndSend(byteArray);
            else
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

        private void SplitAndSend(byte[] messageBytes)
        {
            var messageBodySize = messageBytes.Length;
            var numberOfSubMessages = (int)(messageBodySize / MaxMessageSize);

            if (messageBodySize % MaxMessageSize != 0)
            {
                numberOfSubMessages++;
            }
            
            var sessionId = Guid.NewGuid().ToString();            
            var subMessageNumber = 1;

            //Stream bodyStream = message.GetBody<Stream>();

            for (int streamOffest = 0; streamOffest < messageBodySize; streamOffest += MaxMessageSize)
            {
                var arraySize = (messageBodySize - streamOffest) > MaxMessageSize ? MaxMessageSize : messageBodySize - streamOffest;                
                var subMessageBytes = new byte[arraySize];
                
                //var result = bodyStream.Read(subMessageBytes, 0, (int)arraySize);

                Buffer.BlockCopy(messageBytes, streamOffest, subMessageBytes, 0, arraySize);

                var subMessage = new BrokeredMessage(new MemoryStream(subMessageBytes), true)
                {
                    SessionId = sessionId
                };

                subMessage.Properties.Add("NumberOfSubMessages", numberOfSubMessages);
                subMessage.Properties.Add("SubMessageNumber", subMessageNumber);
               
                fileQueueClient.Send(subMessage);
                subMessageNumber++;
            }
        }

        private QueueClient CreateQueueClient(string queueName)
        {
            if (!namespaceManager.QueueExists(queueName))
            {
                var q = new QueueDescription(queueName);
                q.RequiresSession = true;
                namespaceManager.CreateQueue(q);
            }

            var messagingFactory = MessagingFactory.Create(
                namespaceManager.Address,
                namespaceManager.Settings.TokenProvider);

            return messagingFactory.CreateQueueClient(queueName);
        }
    }
}
