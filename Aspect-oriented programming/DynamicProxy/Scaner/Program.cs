using System.Configuration;
using ScanerService;
using Topshelf;
using Configuration = ScanerService.Helpers.Configuration;
using ScanerService.Interafces;
using ScanerService.Interfaces;

namespace Scaner
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = WindsorConfiguration.Configure();
            var a = container.Resolve<IDirectoryService>();
            var config = new Configuration(ConfigurationManager.AppSettings["Folders"]
                , ConfigurationManager.AppSettings["SuccessFolder"]
                , ConfigurationManager.AppSettings["ErrorFolder"]
                , ConfigurationManager.AppSettings["ProcessingFolder"]
                , ConfigurationManager.AppSettings["FileNamePattern"]
                , int.Parse(ConfigurationManager.AppSettings["TimerTime"])
                , ConfigurationManager.AppSettings["CodeString"]);

            var fileService = container.Resolve<IFileService>(new { config.SuccessFolder });
            HostFactory.Run(x =>
            {
                x.Service<ScanProcessService>(s =>
                {
                    s.ConstructUsing(() =>
                    new ScanProcessService(config,
                    container.Resolve<IDirectoryService>(),
                    container.Resolve<IFileProcessor>(new
                    {
                        fileService,
                        config.SuccessFolder,
                        config.ErrorFolder,
                        config.ProcessingFolder,
                        config.FileNamePattern
                    })));
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
