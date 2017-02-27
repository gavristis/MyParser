using MyParser.Models;

namespace MyParser.DAL.Interfaces
{
    public interface IImageRepository
    {
        Image Get(string url);
        void Add(Image image);
        void AttachEntity(Image image);
    }
}
