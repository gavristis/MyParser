using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Policy;
using MyParser.BLL.Interfaces;
using StructureMap;


namespace MyParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = Container.For<ConsoleRegistry>();
            var sts = container.GetInstance<ISiteTreeService>();
            var ts = container.GetInstance<ITaskService>();
            
            string input;
            //int threads;
            while (true)
            {
                if ((input = Console.ReadLine()) == "start")
                {
                    //{
                    //    var url = "http://" + input;
                    //    ps.AddToQueue(url);

                    //}
                    Stopwatch s1 = new Stopwatch();

                    //Console.WriteLine("Set number of threads");
                    //if (int.TryParse(Console.ReadLine(), out threads))
                    //{
                    //    ps.Run(threads, false);
                    //}
                    //else
                    //{
                    s1.Start();
                    ts.Run("http://wikipedia.org/", false, 1); //externals, depth, threads
                    //sts.BuildTree("http://www.ok-studio.com.ua/", 3);
                    s1.Stop();
                    Console.WriteLine(s1.ElapsedMilliseconds);
                    //sts.BuildTree(1);
                    //}
                }
            }
        }

    }
}
