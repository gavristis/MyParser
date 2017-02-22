using StructureMap;
using Topshelf;

namespace MyParser.Service.Configuration
{
    internal static class ConfigureService
    {
        public static readonly IContainer ServiceContainer;

        static ConfigureService()
        {
            ServiceContainer = Container.For<ConsoleRegistry>();
        }

        internal static void Configure()
        {
            HostFactory.Run(configure =>
            {
                configure.Service<MyService>(service =>
                {
                    service.ConstructUsing(s => new MyService());
                    service.WhenStarted(s =>
                    {
                        s.Start();
                    });
                    service.WhenStopped(s => s.Stop());
                });
                //Setup Account that window service use to run.  
                configure.SetServiceName("MyParser.Service");
                configure.SetDisplayName("MyParser.Service");
                configure.SetDescription("My .Net windows service with Topshelf for parsing HTML pages");

                configure.StartAutomaticallyDelayed();
                configure.RunAsNetworkService();
            });
        }
    }
}
