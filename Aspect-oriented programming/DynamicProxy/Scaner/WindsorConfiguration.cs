using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Logging;
using ScanerService;
using ScanerService.Interafces;
using ScanerService.Interfaces;

namespace Scaner
{
    public static class WindsorConfiguration
    {
        public static IWindsorContainer Configure()
        {
            var container = new WindsorContainer();
            container.Install(
                   new LoggerInstaller()
                );
            
            container.Register(Component.For<IFileProcessor>().
                ImplementedBy<FileProcessor>().
                Interceptors(InterceptorReference.ForType<LogInterceptor>()).Last.LifeStyle.Singleton);

            container.Register(Component.For<IDirectoryService>().
                ImplementedBy<DirectoryService>().
                Interceptors(InterceptorReference.ForType<LogInterceptor>()).Last.LifeStyle.Singleton);

            container.Register(Component.For<IFileService>().
                ImplementedBy<PdfFileService>().
                Interceptors(InterceptorReference.ForType<LogInterceptor>()).Last.LifeStyle.Singleton);

            return container;
        }
    }
}
