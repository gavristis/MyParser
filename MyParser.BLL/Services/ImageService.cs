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
                nodes.Select(s => s.GetAttributeValue("src", null)).Where(s => !String.IsNullOrEmpty(s)).ToList();

            foreach (var img in images)
            {
                Image i = new Image();
                i.Link = img;
                res.Add(i);
            }
            return res;
        }
    }
}
