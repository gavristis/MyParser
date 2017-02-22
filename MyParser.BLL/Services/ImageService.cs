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
            var res = new List<Image>();
            var images = doc.DocumentNode.Descendants("img")
                .Select(s => s.GetAttributeValue("src", null))
                .Where(s => !string.IsNullOrEmpty(s))
                .Distinct();

            foreach (var img in images)
            {
                if (img.Length > 450)
                {
                    Console.WriteLine("URL {0} is too big.", img);
                    continue;
                }

                res.Add(new Image { Link = img });
            }

            return res;
        }
    }
}
