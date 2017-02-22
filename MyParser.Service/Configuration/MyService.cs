using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using MyParser.Service.Services;

namespace MyParser.Service.Configuration
{
    public class MyService
    {
        public ServiceHostBase ServiceHost { get; private set; }

        public void Start()
        {
            if (ServiceHost != null)
            {
                ServiceHost.Close();
            }

            ServiceHost = new StructureMapServiceHost(typeof(ParseService));

            ServiceHost.Open();
        }

        public void Stop()
        {
            if (ServiceHost != null)
            {
                ServiceHost.Close();
                ServiceHost = null;
            }
        }
    }
}
