using System.Configuration;
using ScanerService;
using Topshelf;
using Configuration = ScanerService.Helpers.Configuration;

namespace Scaner
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = WindsorConfiguration.Configure();

            var config = new Configuration(ConfigurationManager.AppSettings["Folders"]
                , ConfigurationManager.AppSettings["SuccessFolder"]
                , ConfigurationManager.AppSettings["ErrorFolder"]
                , ConfigurationManager.AppSettings["ProcessingFolder"]
                , ConfigurationManager.AppSettings["FileNamePattern"]
                , int.Parse(ConfigurationManager.AppSettings["TimerTime"])
                , ConfigurationManager.AppSettings["CodeString"]);

            HostFactory.Run(x =>
            {
                x.Service<ScanProcessService>(s =>
                {
                    s.ConstructUsing(() => new ScanProcessService(config));
                    s.WhenStarted((sc, hostControl) => sc.Start(hostControl));
                    s.WhenStopped((sc, hostControl) => sc.Stop(hostControl));
                });

                x.SetServiceName("ScanService");
                x.SetDisplayName("Scaner Service");
                x.StartAutomaticallyDelayed();
                x.RunAsLocalService();
            });
        }
    }
}
