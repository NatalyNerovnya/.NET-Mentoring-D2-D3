namespace CentralServiceApplication
{
    using Topshelf;
    using QueueClient;
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<AzureQueueClient>();

                x.SetServiceName("CentralService");
                x.SetDisplayName("Central Service");
                x.StartAutomaticallyDelayed();
                x.RunAsLocalService();
            });
        }
    }
}
