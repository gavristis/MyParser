using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyParser.BLL.Interfaces;
using MyParser.Models;

namespace MyParser.BLL.Services
{
    public class CssService : ICssService
    {
        public void SearchCss(Page link)//move return list take(HtmlDoc)
        {
            var nodes = link.HtmlDocument.DocumentNode.Descendants("link")
                .Select(s => new { Rel = s.GetAttributeValue("rel", null), Href = s.GetAttributeValue("href", null) })
                .ToList();

            foreach (var s in nodes)
            {
                if (s.Rel == "stylesheet")
                {
                    link.Css.Add(new Css { Link = s.Href });
                }
            }
        }
    }
}
