using Castle.Windsor;
using Logging;

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

            return container;
        }
    }
}
