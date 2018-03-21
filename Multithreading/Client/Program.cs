using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ChatHelper;

namespace Client
{
    public class Program
    {
        private static string userName;
        private const string host = "127.0.0.1";
        private const int port = 1111;
        private static TcpClient client;
        private static NetworkStream stream;

        static void Main(string[] args)
        {
            userName = NameRepository.GetRandomName();
            client = new TcpClient();
            try
            {
                client.Connect(host, port);
                stream = client.GetStream();

                SendMessage(userName);

                var receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start();
                var messageNumber = MessageRepository.GetRandomMessageNumber();
                Console.WriteLine($"Random User Name = {userName}, ({messageNumber} messages)");

                for (int i = 0; i < messageNumber; i++)
                {
                    Thread.Sleep(MilisecondsRepository.GetRandomMilisecondsNumber());
                    SendMessage(MessageRepository.GetRandomMessage());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Dispose();
                Disconnect();
            }
        }

        private static void SendMessage(string message)
        {
                var data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
        }
        
        private static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    var data = new byte[64];
                    var builder = new StringBuilder();
                    do
                    {
                        var bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (stream.DataAvailable);

                    var message = builder.ToString();
                    Console.WriteLine(message);
                }
                catch
                {
                    Disconnect();
                }
            }
        }

        private static void Disconnect()
        {
            stream?.Close();
            client?.Close();
        }
    }
}