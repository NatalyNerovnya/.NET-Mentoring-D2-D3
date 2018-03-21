using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class Server
    {
        private static TcpListener tcpListener;
        private List<Client> clients;

        public Server()
        {
            clients = new List<Client>();
        }

        public void AddConnection(Client client)
        {
            clients.Add(client);
        }

        public void RemoveConnection(string id)
        {
            var client = clients.FirstOrDefault(c => c.Id == id);
            clients?.Remove(client);
        }
       
        public void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 1111);
                tcpListener.Start();
                Console.WriteLine("Server is started. Waiting for connections...");

                while (true)
                {
                    var tcpClient = tcpListener.AcceptTcpClient();

                    var client = new Client(tcpClient, this);
                    var clientThread = new Thread(client.Process);
                    Console.WriteLine($"Client with id = {client.Id} is on the {clientThread.ManagedThreadId} thread.");
                    clientThread.Start();
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
            foreach (var client in clients.Where(client => client.Id != id))
            {
                client.Stream.Write(data, 0, data.Length); 
            }
        }

        public void Disconnect()
        {
            tcpListener.Stop();

            foreach (var client in clients)
            {
                client.Close();
            }
        }
    }
}
