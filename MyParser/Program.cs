using System;
using System.Text.RegularExpressions;
using MyParser.BLL.Interfaces;
using StructureMap;


namespace MyParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = Container.For<ConsoleRegistry>();
            var ps = container.GetInstance<IPageService>();
            var sts = container.GetInstance<ISiteTreeService>();
            string input;
            int threads;
            //while ((input = Console.ReadLine())!="start")
            //{
            //    var url = "http://" + input;
            //    ps.AddToQueue(url);

            //}
            ps.AddToQueue("http://www.ok-studio.com.ua/");
            //Console.WriteLine("Set number of threads");
            //if (int.TryParse(Console.ReadLine(), out threads))
            //{
            //    ps.Run(threads, false);
            //}
            //else
            //{

            ps.Run(10, false, 200);
            sts.BuildTree("http://www.ok-studio.com.ua/", 3);
            //}
            //Main(args);
        }

    }
}
