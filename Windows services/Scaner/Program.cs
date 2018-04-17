using ScanerService;
using Topshelf;

namespace Scaner
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<ScanProcessService>();
                x.SetServiceName("ScanService");
                x.SetDisplayName("Scaner Service");
                x.StartAutomaticallyDelayed();
                x.RunAsLocalService();
            });
        }
    }
}
