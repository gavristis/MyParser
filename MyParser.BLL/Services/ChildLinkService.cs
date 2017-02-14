using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyParser.BLL.Interfaces;
using MyParser.Models;

namespace MyParser.BLL.Services
{
    public class ChildLinkService : IChildLinkService
    {
        private readonly ITaskService _taskService;

        public ChildLinkService(ITaskService taskService)
        {
            _taskService = taskService;
        }
        public void GetLinks(Page link)//move => bool getinternals
        {
            var nodes = link.HtmlDocument.DocumentNode.Descendants("a");
            var links = nodes.Select(s => s.GetAttributeValue("href", null))
                .Where(s => s != link.Uri.AbsoluteUri && !string.IsNullOrEmpty(s) && !s.StartsWith("#") && !s.StartsWith("/")).Distinct().ToList();

            foreach (var s in links)
            {
                _taskService.AddToQueue(s, link);
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
                            _taskService.AddToQueue(s1, link);
                        }
                        else if (link.Uri.AbsoluteUri.EndsWith("/"))
                        {
                            string substring = s.Substring(1, s.Length - 1);
                            var s1 = link.Uri.AbsoluteUri + substring;
                            //link.ChildLinks.Add(s1);
                            _taskService.AddToQueue(s1, link);
                        }
                        else
                        {

                            var s1 = link.Uri.Scheme + "://" + link.Uri.Host + s;
                            _taskService.AddToQueue(s1, link);

                        }

                    }

                }
                else
                {
                    //link.ChildLinks.Add(s);
                    _taskService.AddToQueue(s, link);
                }
            }
        }
    }
}
