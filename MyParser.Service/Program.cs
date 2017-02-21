using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace MyParser.Service
{
    class Program
    {
        public class MyService
        {
            public void Start()
            {
                
                // write code here that runs when the Windows Service starts up.  
            }
            public void Stop()
            {
                // write code here that runs when the Windows Service stops.  
            }
        }
        internal static class ConfigureService
        {
            internal static void Configure()
            {
                HostFactory.Run(configure =>
                {
                    configure.Service<MyService>(service =>
                    {
                        service.ConstructUsing(s => new MyService());
                        service.WhenStarted(s => s.Start());
                        service.WhenStopped(s => s.Stop());
                    });
                    //Setup Account that window service use to run.  
                    configure.RunAsLocalSystem();
                    configure.SetServiceName("MyParser.Service");
                    configure.SetDisplayName("MyParser.Service");
                    configure.SetDescription("My .Net windows service with Topshelf for parsing HTML pages");
                });
            }
        }
        static void Main(string[] args)
        {
            ConfigureService.Configure();
        }
    }
}
