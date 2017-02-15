using System.Collections.Generic;
using HtmlAgilityPack;
using MyParser.Models;

namespace MyParser.BLL.Interfaces
{
    public interface IImageService
    {
        List<Image> SearchImages(HtmlDocument doc);
    }
}
