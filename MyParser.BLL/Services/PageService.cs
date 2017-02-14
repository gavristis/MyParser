using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MyParser.BLL.Interfaces;
using MyParser.BLL.Models;
using MyParser.DAL.Interfaces;
using MyParser.Models;

namespace MyParser.BLL.Services
{
    public class PageService : IPageService
    {
        private readonly object _lock = new object();
        private readonly IUnitOfWork _unitOfWork;//to task service
        public static List<string> visitedPages = new List<string>();//to task service
        public ConcurrentQueue<PageRelationDto> queue = new ConcurrentQueue<PageRelationDto>();//to task service

        public PageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public void Execute(bool withExternals, int maxDepth)//TODO:Modernize producer-consumer(AutoResetEvent) to TaskService
        {
            PageRelationDto dto;
            while (true)
            {
                Console.WriteLine("Thread #{0} started executing", Thread.CurrentThread.Name);
                if (queue.TryDequeue(out dto))
                {
                    Console.WriteLine("Thread #{0} has taken url: {1}", Thread.CurrentThread.Name, dto.Url);

                    try
                    {
                        var depth = dto.Parent == null ? 0 : dto.Parent.Depth + 1;
                        if (depth > maxDepth)
                        {
                            continue;
                        }
                        var p = Parse(dto.Url, withExternals, depth);
                        p.Parent = dto.Parent;
                        lock (_lock)
                        {
                            _unitOfWork.PageRepository.Add(p);
                            _unitOfWork.Save();
                        }
                        //foreach (p.Links){AddToQueue()};
                    }
                    catch (WebException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
                else
                {
                    Thread.Sleep(2000);
                    if (queue.IsEmpty)
                    {
                        break;
                    }
                }
            }
        }

        public Page Parse(string url, bool withExternals, int depth)
        {
            Page page = new Page();
            page.HtmlDocument = new HtmlDocument();
            page.Css = new List<Css>();
            page.Images = new List<Image>();
            page.Url = url;            
            MeasureTime(page);
            page.Html = page.HtmlDocument.DocumentNode.OuterHtml;                        
            MeasureSize(page);
            SearchCss(page);
            SearchImages(page);
            page.Depth = depth;
            if (withExternals == false)
            {
                GetInternals(page);
            }
            else
            {
                GetLinks(page);
            }                      
            return page;
        }

        public void AddToQueue(string url)
        {
            AddToQueue(url, null);
        }

        public void MeasureTime(Page link)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            link.HtmlDocument.LoadHtml(new WebClient().DownloadString(link.Url));
            stopWatch.Stop();
            //Console.WriteLine("Page was loaded in {0} milliseconds", stopWatch.ElapsedMilliseconds);
            link.LoadTime = stopWatch.ElapsedMilliseconds;
        }

        public void MeasureSize(Page link)
        {
            link.Size = Encoding.Unicode.GetByteCount(link.Html);
            //Console.WriteLine("Page has size of {0} bytes", link.Size);
        }

        public void SearchCss(Page link)//move return list(HtmlDoc)
        {
            var nodes = link.HtmlDocument.DocumentNode.Descendants("link")
                .Select(s => new {Rel = s.GetAttributeValue("rel", null), Href = s.GetAttributeValue("href", null)})
                .ToList();

            foreach (var s in nodes)
            {
                if (s.Rel == "stylesheet")
                {
                    link.Css.Add(new Css {Link = s.Href});
                }
            }
        }

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

        public void GetLinks(Page link)//move => bool getinternals
        {
            var nodes = link.HtmlDocument.DocumentNode.Descendants("a");
            var links = nodes.Select(s => s.GetAttributeValue("href", null))
                .Where(s => s!=link.Uri.AbsoluteUri && !String.IsNullOrEmpty(s) && !s.StartsWith("#") && !s.StartsWith("/")).Distinct().ToList();

            foreach (var s in links)
            {
                AddToQueue(s, link);
            }
        }

        public void GetInternals(Page link)
        {
            var nodes = link.HtmlDocument.DocumentNode.Descendants("a");
            var links = nodes.Select(s => s.GetAttributeValue("href", null))
                .Where(s => s!=null && s!="#" && s!="/" && (s.StartsWith("/") || s.Contains(link.Uri.AbsoluteUri)) ).Distinct().ToList();
            foreach (var s in links)
            {

                if (s.StartsWith("/"))
                {
                    {
                        if (s.StartsWith("//"))
                        {
                            var s1 = "http:" + s;
                            AddToQueue(s1,link);
                        }
                        else if (link.Uri.AbsoluteUri.EndsWith("/"))
                        {
                            string substring = s.Substring(1, s.Length - 1);
                            var s1 = link.Uri.AbsoluteUri + substring;
                            //link.ChildLinks.Add(s1);
                            AddToQueue(s1, link);
                        }
                        else
                        {

                                var s1 = link.Uri.Scheme+ "://" + link.Uri.Host + s;
                                AddToQueue(s1, link);
                            
                        }
                        
                    }

                }
                else
                {
                    //link.ChildLinks.Add(s);
                    AddToQueue(s, link);
                }
            }
        }

        public void Run(int threadNum, bool withExternals, int depth)// to task service
        {
            var threads = new List<Task>();
            for (int i = 0; i < threadNum; i++)
            {
                //lock (_syncRoot)
                //{
                var name = i.ToString();
                Task t = Task.Factory.StartNew(() =>
                {
                    Thread.CurrentThread.Name = name;
                    Execute(withExternals, depth);
                });
                
                threads.Add(t);
                //}
            }

            //q.CompleteAdding(); // останавливаем

            Task.WaitAll(threads.ToArray());
        }

        public void AddToQueue(string url, Page parent)//task service
        {
            if (!visitedPages.Contains(url))
            {
                lock (_lock)
                {
                    if (!visitedPages.Contains(url))
                    {
                        PageRelationDto dto = new PageRelationDto
                        {
                            Url = url,
                            Parent = parent
                        };
                        queue.Enqueue(dto);
                        visitedPages.Add(url);
                    }
                }
            }
        }
    }
}
