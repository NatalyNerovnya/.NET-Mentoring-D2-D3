using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace KeyGenerator
{
    class Program
    {
        [STAThreadAttribute]
        public static void Main(string[] args)
        {
            var key = GenerateKey();

            Clipboard.SetText(key);

            Console.WriteLine(key);
            Console.WriteLine("This key is copied to clickboard");
            Console.ReadLine();

        }
        public static string GenerateKey()
        {
            NetworkInterface networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(); ;
            byte[] addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes(); ;
            DateTime dateTime = DateTime.Now.Date;

            var dateBytes = BitConverter.GetBytes(dateTime.ToBinary());
            var source = addressBytes
                .Select((b, index) => b ^ dateBytes[index])
                .Select(x => x <= 999 ? x * 10 : x)
                .ToArray();

            return string.Join("-", source);
        }
    }
}
