using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using MyParser.BLL.Interfaces;
using MyParser.Models;

namespace MyParser.BLL.Services
{
    public class ImageService : IImageService
    {
        public List<Image> SearchImages(HtmlDocument doc)
        {            
            var  res = new List<Image>();
            var nodes = doc.DocumentNode.Descendants("img");
            var images =
                nodes.Select(s => s.GetAttributeValue("src", null)).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();

            foreach (var img in images)
            {
                /*if (img.Length > 450)
                {
                    Console.WriteLine("URL {0} is too big.", img);
                    continue;
                }*/
                res.Add(new Image { Link = img });
            }

            return res;
        }
    }
}
