using MyParser.Models;

namespace MyParser.BLL.Interfaces
{
    public interface IChildLinkService
    {
        void GetInternals(Page link);
        void GetLinks(Page link);
    }
}
