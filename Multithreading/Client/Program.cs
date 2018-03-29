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
        private static string _userName;
        private static readonly string Host = ConfigurationManager.AppSettings["ServerHost"];
        private static readonly int Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
        private static TcpClient _client;
        private static NetworkStream _stream;
        private static readonly bool IsTplVersion = Convert.ToBoolean(ConfigurationManager.AppSettings["TPLVersion"]);
        private static Task _recievertask;

        static void Main(string[] args)
        {
            _userName = NameRepository.GetRandomName();
            _client = new TcpClient();
            Console.CancelKeyPress += CancelKeyHandler;
            try
            {
                _client.Connect(Host, Port);
                _stream = _client.GetStream();

                SendMessage(_userName);

                if (!IsTplVersion)
                {
                    var recieverThread = new Thread(ReceiveMessage);
                    recieverThread.Start();
                }
                else
                {
                    _recievertask = Task.Factory.StartNew(ReceiveMessage);
                }

                var messageNumber = MessageRepository.GetRandomMessageNumber();
                Console.WriteLine($"Random User Name = {_userName}, ({messageNumber} messages)");

                for (int i = 0; i < messageNumber; i++)
                {
                    Thread.Sleep(MilisecondsRepository.GetRandomMilisecondsNumber());
                    SendMessage(MessageRepository.GetRandomMessage());
                }

                if (IsTplVersion)
                {
                    _recievertask.Wait();
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
            _stream.Write(data, 0, data.Length);
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
                        var bytes = _stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (_stream.DataAvailable);

                    var message = builder.ToString();
                    Console.WriteLine(message);
                }
                catch
                {
                    Disconnect();
                }
            }
        }

        private static void CancelKeyHandler(object sender, ConsoleCancelEventArgs args)
        {
            SendMessage("END");

            Disconnect();
        }


        private static void Disconnect()
        {
            _stream?.Close();
            _client?.Close();
            Environment.Exit(0);
        }
    }
}