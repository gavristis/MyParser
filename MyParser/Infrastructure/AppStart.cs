using System;
using MyParser.BLL.Interfaces;
using StructureMap;

namespace MyParser.Infrastructure
{
    class AppStart
    {
        public static void Start()
        {
            var container = Container.For<ConsoleRegistry>();
            var sts = container.GetInstance<ISiteTreeService>();
            var ts = container.GetInstance<ITaskService>();


            while (true)
            {
                Console.WriteLine("Enter starting URL (with http)");
                var input = Console.ReadLine();
                Console.WriteLine("Enter depth (of either parsing or tree building) default value: 2");
                int depth;
                if (int.TryParse(Console.ReadLine(), out depth))
                {

                }
                else
                {
                    depth = 2;
                }
                if (!Uri.IsWellFormedUriString(input, UriKind.Absolute)) continue;
                Console.WriteLine(
                    "Select an operation: 1 - Parse, 2 - Build site tree(if one exists) Any other button - Exit");
                var caseSwitch = Console.ReadLine();
                switch (caseSwitch)
                {
                    case "1":
                        Console.WriteLine("Select a number of working threads (default 10)");
                        int threads;
                        if (int.TryParse(Console.ReadLine(), out threads))
                        {
                            Console.WriteLine("Include external links? 1 - yes, skip - no");
                            var withExternal = Console.ReadLine();
                            ts.Run(input, withExternal == "1", depth, threads);
                        }
                        else
                        {
                            Console.WriteLine("Include external links? 1 - yes, skip - no");
                            var withExternal = Console.ReadLine();
                            ts.Run(input, withExternal == "1", depth);
                        }
                        break;
                    case "2":
                        sts.BuildTree(input, depth);
                        break;
                    default:
                        Console.WriteLine("Execution Finished");
                        break;
                }

                ////sts.BuildTree("http://www.ok-studio.com.ua/", 4);
                //s1.Stop();
                //Console.WriteLine(s1.ElapsedMilliseconds);
                ////sts.BuildTree(2);
                ////}
                //sts.BuildTree("http://www.facebook.com/", 4);

            }
        }
    }
}
