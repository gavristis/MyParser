using System;
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
                .Where(s => s != null && s != "#" && s != "/" && !s.Contains(".jpg") && !s.Contains("mailto:") && !s.Contains("{")).Distinct().ToList();
            foreach (var s in links)
            {
                if (s.StartsWith("/"))
                {

                    if (s.StartsWith("//"))
                    {
                        var s1 = link.Uri.Scheme + ":" + s;
                        link.ChildUlrs.Add(s1);
                    }
                    else
                    {
                        var s1 = link.Uri.Scheme + "://" + link.Uri.Host + s;
                        //link.ChildLinks.Add(s1);
                        link.ChildUlrs.Add(s1);
                    }

                }
                else if (!s.StartsWith("http"))
                {
                    if (s.Contains(link.Uri.GetLeftPart(UriPartial.Authority).Replace("www.", "").Replace("http://", "")))
                    {
                        var s1 = link.Uri.Scheme + "://" + s;
                        link.ChildUlrs.Add(s1);
                    }
                    else
                    {
                        var s2 = link.Uri.Scheme + "://" + link.Uri.Host + s;
                        link.ChildUlrs.Add(s2);
                    }
                }
                else
                {
                    link.ChildUlrs.Add(s);
                }
            }
        }
        public void GetInternals(Page link)
        {
            var nodes = link.HtmlDocument.DocumentNode.Descendants("a");
            var links = nodes.Select(s => s.GetAttributeValue("href", null))
                .Where(s => s != null && s != "#" && s != "/" && (s.StartsWith("/") || s.Contains(link.Uri.GetLeftPart(UriPartial.Authority).Replace("www.", "").Replace("http://", ""))) && !s.Contains(".jpg") && !s.Contains("mailto:") && !s.Contains("{")).Distinct().ToList();
            foreach (var s in links)
            {
                if (s.StartsWith("/"))
                {

                    if (s.StartsWith("//"))
                    {
                        var s1 = link.Uri.Scheme+":" + s;
                        link.ChildUlrs.Add(s1);                        
                    }
                    else 
                    {                        
                        var s1 = link.Uri.Scheme+"://"+link.Uri.Host + s;
                        //link.ChildLinks.Add(s1);
                        link.ChildUlrs.Add(s1);
                    }

                }
                else if (!s.StartsWith("http"))
                {
                    if (s.Contains(link.Uri.GetLeftPart(UriPartial.Authority).Replace("www.", "").Replace("http://", "")))
                    {
                        var s1 = link.Uri.Scheme + "://" + s;
                        link.ChildUlrs.Add(s1);
                    }
                    else
                    {
                        var s2 = link.Uri.Scheme + "://" + link.Uri.Host + s;
                        link.ChildUlrs.Add(s2);
                    }
                }
                else
                {
                    link.ChildUlrs.Add(s);
                }
            }
        }

        public bool IsLinkInternal(string parent, string child)
        {
            var uri = new Uri(parent);
            return child.Contains(uri.GetLeftPart(UriPartial.Authority).Replace("www.", "").Replace("http://", ""));
        }
    }
}
