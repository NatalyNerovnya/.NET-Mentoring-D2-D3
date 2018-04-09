using System;

namespace PowerManagementConsoleTest
{
    class Program
    {
        public static void Main(string[] args)
        {
            var pw = new PowerManagement.PowerManagement();

            var lastSleepTime = pw.GetLastSleepTime();
            var lastWakeTime = pw.GetLastWakeTime();
            var systemBatteryState = pw.GetSystemBatteryState();
            var systemPowerInformation = pw.GetSystemPowerInformation();

            Console.WriteLine($"Last sleep time: {pw.GetLastSleepTime()}");
            Console.WriteLine($"Last wake time:  {pw.GetLastWakeTime()}");
            Console.WriteLine($"System battery state: {pw.GetSystemBatteryState()}");
            Console.WriteLine($"System power information: {pw.GetSystemPowerInformation()}");

            Console.ReadKey();
            pw.Sleep();

            Console.ReadKey();
        }
    }
}
