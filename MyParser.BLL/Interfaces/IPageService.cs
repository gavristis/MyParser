using MyParser.Models;

namespace MyParser.BLL.Interfaces
{
    public interface IPageService
    {
        void MeasureTime(Page link);

        void MeasureSize(Page link);
    }
}
