using MyParser.Models;

namespace MyParser.BLL.Interfaces
{
    public interface IPageService
    {
        void Run(int i, bool b, int depth);
        void AddToQueue(string url);
        Page Parse(string url, bool withExternals, int depth);
    }
}
