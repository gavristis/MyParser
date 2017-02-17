using System.Linq;
using MyParser.BLL.Interfaces;
using MyParser.Models;

namespace MyParser.BLL.Services
{
    public class ChildLinkService : IChildLinkService
    {
        public void GetLinks(Page link)//move => bool getinternals
        {
            var nodes = link.HtmlDocument.DocumentNode.Descendants("a");
            var links = nodes.Select(s => s.GetAttributeValue("href", null))
                .Where(s => s != null && s != "#" && s != "/" && !s.Contains(".jpg")).Distinct().ToList();

            foreach (var s in links)
            {
                link.ChildUlrs.Add(s);
            }
        }
        public void GetInternals(Page link)
        {
            var nodes = link.HtmlDocument.DocumentNode.Descendants("a");
            var links = nodes.Select(s => s.GetAttributeValue("href", null))
                .Where(s => s != null && s != "#" && s != "/" && (s.StartsWith("/") || s.Contains(link.Uri.Host)) && !s.Contains(".jpg")).Distinct().ToList();
            foreach (var s in links)
            {

                if (s.StartsWith("/"))
                {
                    {
                        if (s.StartsWith("//"))
                        {
                            var s1 = "http:" + s;
                            link.ChildUlrs.Add(s1); ;
                        }
                        else if (link.Uri.AbsoluteUri.EndsWith("/"))
                        {
                            string substring = s.Substring(1, s.Length - 1);
                            var s1 = link.Uri.AbsoluteUri + substring;
                            //link.ChildLinks.Add(s1);
                            link.ChildUlrs.Add(s1); ;
                        }
                        else
                        {

                            var s1 = link.Uri.Scheme + "://" + link.Uri.Host + s;
                            link.ChildUlrs.Add(s1);

                        }

                    }

                }
                else
                {
                    link.ChildUlrs.Add(s);
                }
            }
        }
    }
}
