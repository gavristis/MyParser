using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyParser.Service.Configuration;
using Topshelf;

namespace MyParser.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigureService.Configure();
        }
    }
}
