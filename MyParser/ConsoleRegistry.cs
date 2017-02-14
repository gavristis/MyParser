using MyParser.BLL.Interfaces;
using MyParser.BLL.Services;
using MyParser.DAL.Interfaces;
using MyParser.DAL.Repositories;
using StructureMap;

namespace MyParser
{
    public class ConsoleRegistry : Registry
    {
        public ConsoleRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
            // requires explicit registration; doesn't follow convention
            For<IUnitOfWork>().Use<UnitOfWork>();
            For<IPageService>().Use<PageService>();
            For<ISiteTreeService>().Use<SiteTreeService>();
            For<IChildLinkService>().Use<ChildLinkService>();
            For<ICssService>().Use<CssService>();
            For<IImageService>().Use<ImageService>();
            For<ITaskService>().Use<TaskService>();
            For<IParserService>().Use<ParserService>();
        }
    }
}