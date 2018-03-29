using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        private static TcpListener _tcpListener;
        private readonly List<Client> _clients;
        private readonly bool _isTplVersion;
        private static readonly int Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);

        public Server()
        {
            _clients = new List<Client>();
            _isTplVersion = Convert.ToBoolean(ConfigurationManager.AppSettings["TPLVersion"]);
        }

        public void AddConnection(Client client)
        {
            _clients.Add(client);
        }

        public void RemoveConnection(string id)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            _clients?.Remove(client);
        }
       
        public void Listen()
        {
            try
            {
                _tcpListener = new TcpListener(IPAddress.Any, Port);
                _tcpListener.Start();
                Console.WriteLine("Server is started. Waiting for connections...");

                while (true)
                {
                    var tcpClient = _tcpListener.AcceptTcpClient();

                    var client = new Client(tcpClient, this);
                    if (!_isTplVersion)
                    {
                        var clientThread = new Thread(client.Process);
                        Console.WriteLine(
                            $"Client with id = {client.Id} is on the {clientThread.ManagedThreadId} thread.");
                        clientThread.Start();
                    }
                    else
                    {
                        Task.Factory.StartNew(client.Process);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        public void BroadcastMessage(string message, string id)
        {
            var data = Encoding.Unicode.GetBytes(message);
            foreach (var client in _clients.Where(client => client.Id != id))
            {
                client.Stream.Write(data, 0, data.Length); 
            }
        }

        public void Disconnect()
        {
            _tcpListener.Stop();

            foreach (var client in _clients)
            {
                client.Close();
            }

            Environment.Exit(0);
        }
    }
}
