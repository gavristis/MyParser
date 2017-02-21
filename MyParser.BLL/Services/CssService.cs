using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using MyParser.BLL.Interfaces;
using MyParser.Models;

namespace MyParser.BLL.Services
{
    public class CssService : ICssService
    {

        public List<Css> SearchCss(HtmlDocument doc)
        {
            var res = new List<Css>();
            var nodes = doc.DocumentNode.Descendants("link")
                .Select(s => new { Rel = s.GetAttributeValue("rel", null), Href = s.GetAttributeValue("href", null) }).Distinct()
                .ToList();

            foreach (var s in nodes)
            {
                
                if (s.Rel == "stylesheet")
                {
                    if (s.Href.Length > 450)
                    {
                        Console.WriteLine("URL {0} is too big.", s.Href);
                        continue;
                    }
                    res.Add(new Css { Link = s.Href });
                }
            }
            return res;
        }
    }
}
