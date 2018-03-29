using System;

namespace Server
{
    class Program
    {
        private static Server _server;

        static void Main(string[] args)
        {
            try
            {
                _server = new Server();
                _server.Listen();
            }
            catch (Exception ex)
            {
                _server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
