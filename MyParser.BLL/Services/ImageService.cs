using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyParser.BLL.Interfaces;
using MyParser.Models;

namespace MyParser.BLL.Services
{
    public class ImageService : IImageService
    {
        public void SearchImages(Page link)//move ^ same
        {
            var nodes = link.HtmlDocument.DocumentNode.Descendants("img");
            var images =
                nodes.Select(s => s.GetAttributeValue("src", null)).Where(s => !String.IsNullOrEmpty(s)).ToList();

            foreach (var img in images)
            {
                Image i = new Image();
                i.Link = img;
                link.Images.Add(i);
            }


        }
    }
}
