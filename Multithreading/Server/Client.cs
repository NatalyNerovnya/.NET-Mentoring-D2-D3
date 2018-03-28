﻿using System;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Client
    {
        public string Id { get; private set; }
        public NetworkStream Stream { get; private set; }

        private string userName;
        private readonly TcpClient client;
        private readonly Server server;

        public Client(TcpClient tcpClient, Server serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                this.userName = GetMessage();

                var message = $"{this.userName} enter the chat";
                server.BroadcastMessage(message, Id);
                Console.WriteLine(message);
                while (true)
                {
                    var mes = GetMessage();
                    if (mes == "END")
                    {
                        RecieveAndUpdate($"{userName}: leave the chat");
                        break;
                    }

                    RecieveAndUpdate($"{userName}:{mes}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                server.RemoveConnection(Id);
                Close();
            }
        }

        public void Close()
        {
            Stream?.Close();
            client?.Close();
        }

        private void RecieveAndUpdate(string message)
        {
            Console.WriteLine(message);
            server.BroadcastMessage(message, Id);
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
