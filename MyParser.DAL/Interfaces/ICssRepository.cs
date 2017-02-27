using MyParser.Models;

namespace MyParser.DAL.Interfaces
{
    public interface ICssRepository
    {
        Css Get(string url);
        void Add(Css css);
        void AttachEntity(Css css);
    }
}
