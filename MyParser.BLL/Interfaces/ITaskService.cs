using MyParser.Models;

namespace MyParser.BLL.Interfaces
{
    public interface ITaskService
    {
        void Execute(bool withExternals, int maxDepth);
        void Run(int i, bool b, int depth);
        void AddToQueue(string url);
        void AddToQueue(string url, Page parent);
    }
}
