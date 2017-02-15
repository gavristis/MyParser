using System.Collections.Generic;
using HtmlAgilityPack;
using MyParser.Models;

namespace MyParser.BLL.Interfaces
{
    public interface ICssService
    {
        List<Css> SearchCss(HtmlDocument doc);
    }
}
