using System.ServiceModel;

namespace MyParser.Service.Services
{
    [ServiceContract]
    public interface IParseService
    {
        [OperationContract]
        long Parse(string url, bool withExternals, int depth, int threadNum = 10);
    }
}