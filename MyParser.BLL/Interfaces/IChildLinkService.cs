using MyParser.Models;

namespace MyParser.BLL.Interfaces
{
    public interface IChildLinkService
    {
        void GetLinks(Page link, bool withExternal);
    }
}
