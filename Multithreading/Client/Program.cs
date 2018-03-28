using System;
using System.Configuration;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private static bool isTplVersion = Convert.ToBoolean(ConfigurationManager.AppSettings["TPLVersion"]);
        private static Thread _recieverThread;
        private static CancellationTokenSource cts;


        static void Main(string[] args)
        {
            userName = NameRepository.GetRandomName();
            client = new TcpClient();
            cts = new CancellationTokenSource();
            Console.CancelKeyPress += cancelKeyHandler;
            try
            {
                client.Connect(host, port);
                stream = client.GetStream();

                SendMessage(userName);

                if (!isTplVersion)
                {
                    _recieverThread = new Thread(ReceiveMessage);
                    _recieverThread.Start();
                }
                else
                {
                    Task.Factory.StartNew(ReceiveMessage, cts.Token);
                }

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

        private static void cancelKeyHandler(object sender, ConsoleCancelEventArgs args)
        {
            SendMessage("END");

            Disconnect();
        }


        private static void Disconnect()
        {
            _recieverThread.Abort();
            cts.Cancel();
            stream?.Close();
            client?.Close();
        }
    }
}