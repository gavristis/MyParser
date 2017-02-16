using MyParser.Models;

namespace MyParser.BLL.Interfaces
{
    public interface ITaskService
    {
        void Execute(bool withExternals, int maxDepth);
        void Run(string url, bool withExternals, int depth, int threads = 10);
        void AddToQueue(string url);
        void AddToQueue(string url, Page parent);
    }
}
