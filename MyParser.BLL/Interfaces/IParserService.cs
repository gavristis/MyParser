using MyParser.Models;

namespace MyParser.BLL.Interfaces
{
    public interface IParserService
    {
        Page Parse(string url, bool withExternals, int depth);
    }
}
