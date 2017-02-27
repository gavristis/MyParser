using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyParser.BLL.Interfaces;
using StructureMap;

namespace MyParser.UnitTests
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void ParsedPageNotNull()
        {
            var container = Container.For<ConsoleRegistry>();
            var ps = container.GetInstance<IParserService>();
            var page = ps.Parse("https://www.facebook.com/", false, 1);
            Assert.IsNotNull(page);
        }
    }
}
