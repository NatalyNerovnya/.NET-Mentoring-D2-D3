using System;
using System.Threading;

namespace Server
{
    class Program
    {
        private static Server server;
        private static Thread listenThread;
        static void Main(string[] args)
        {
            try
            {
                server = new Server();
                server.Listen();
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
