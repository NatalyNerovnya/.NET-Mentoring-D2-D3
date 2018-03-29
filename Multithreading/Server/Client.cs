using System;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Client
    {
        public string Id { get; }
        public NetworkStream Stream { get; private set; }

        private string _userName;
        private readonly TcpClient _client;
        private readonly Server _server;

        public Client(TcpClient tcpClient, Server serverObject)
        {
            Id = Guid.NewGuid().ToString();
            _client = tcpClient;
            _server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = _client.GetStream();
                _userName = GetMessage();

                var message = $"{_userName} enter the chat";
                _server.BroadcastMessage(message, Id);
                Console.WriteLine(message);
                while (true)
                {
                    var mes = GetMessage();
                    if (mes == "END")
                    {
                        RecieveAndUpdate($"{_userName}: leave the chat");
                        break;
                    }

                    RecieveAndUpdate($"{_userName}:{mes}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _server.RemoveConnection(Id);
                Close();
            }
        }

        public void Close()
        {
            Stream?.Close();
            _client?.Close();
        }

        private void RecieveAndUpdate(string message)
        {
            Console.WriteLine(message);
            _server.BroadcastMessage(message, Id);
        }

        private string GetMessage()
        {
            var data = new byte[64];
            var builder = new StringBuilder();
            do
            {
                var bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            } while (Stream.DataAvailable);

            return builder.ToString();
        }
    }
}
