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
            int threads;
            //while ((input = Console.ReadLine())!="start")
            //{
            //    var url = "http://" + input;
            //    ps.AddToQueue(url);

            //}
            ts.AddToQueue("http://www.ok-studio.com.ua/");
            //Console.WriteLine("Set number of threads");
            //if (int.TryParse(Console.ReadLine(), out threads))
            //{
            //    ps.Run(threads, false);
            //}
            //else
            //{

            ts.Run(false, 4, 15); //externals, depth, threads
            //sts.BuildTree("http://www.ok-studio.com.ua/", 3);
            sts.BuildTree("http://www.ok-studio.com.ua/zal-201", 4);
            //}
            //Main(args);
        }

    }
}
