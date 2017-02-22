using System;
using System.Collections.Generic;
using System.Linq;
using MyParser.BLL.Interfaces;
using MyParser.Models;

namespace MyParser.BLL.Services
{
    public class ChildLinkService : IChildLinkService
    {
        public void GetLinks(Page link, bool withExternal)//move => bool getinternals
        {
            var nodes = link.HtmlDocument.DocumentNode.Descendants("a");
            var links = new List<string>();
            if (withExternal)
            {
                links = nodes.Select(s => s.GetAttributeValue("href", null))
                    .Where(
                        s =>
                            s != null && s != "#" && s != "/" && !s.Contains(".jpg") && !s.Contains("mailto:") &&
                            !s.Contains(".pdf") && !s.Contains("{") && !s.Contains("..")).Distinct().ToList();

            }
            else
            {
                links = nodes.Select(s => s.GetAttributeValue("href", null))
                    .Where(
                        s =>
                            s != null && s != "#" && s != "/" &&
                            (s.StartsWith("/") ||
                             s.Contains(
                                 link.Uri.GetLeftPart(UriPartial.Authority)
                                     .Replace("www.", "")
                                     .Replace("https://", "")
                                     .Replace("http://", ""))) && !s.Contains(".jpg") && !s.Contains(".pdf") &&
                            !s.Contains("mailto:") && !s.Contains("{") && !s.Contains("..")).Distinct().ToList();

            }
            foreach (var s in links)
            {
                if (s.StartsWith("/"))
                {

                    if (s.StartsWith("//"))
                    {
                        var s1 = link.Uri.Scheme + ":" + s;
                        link.ChildUlrs.Add(s1);
                    }
                    else if (link.Uri.Host.EndsWith("/"))
                    {
                        var s1 = link.Uri.Scheme + "://" + link.Uri.Host + s.Substring(1);
                        //link.ChildLinks.Add(s1);
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
                    if (s.Contains(link.Uri.GetLeftPart(UriPartial.Authority).Replace("www.", "").Replace("https://", "").Replace("http://", "")))
                    {
                        var s1 = link.Uri.Scheme + "://" + s;
                        link.ChildUlrs.Add(s1);
                    }
                    else if (!link.Uri.Host.EndsWith("/"))
                    {
                        var s1 = link.Uri.Scheme + "://" + link.Uri.Host + "/" + s;
                        //link.ChildLinks.Add(s1);
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
    }
}
