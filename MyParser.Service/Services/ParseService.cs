using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Activation;
using MyParser.BLL.Interfaces;

namespace MyParser.Service.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ParseService : IParseService
    {
        private readonly ITaskService _taskService;

        public ParseService(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public long Parse(string url, bool withExternals, int depth, int threadNum = 10)
        {
            Stopwatch s1 = new Stopwatch();

            s1.Start();

            _taskService.Run(url, withExternals, depth, threadNum);

            s1.Stop();

            return s1.ElapsedMilliseconds;
        }
    }
}