using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyParser.BLL.Interfaces;
using MyParser.BLL.Models;
using MyParser.DAL.Interfaces;
using MyParser.Models;
using NLog;

namespace MyParser.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IParserService _parserService;
        public static HashSet<string> VisitedPages = new HashSet<string>();
        public ConcurrentQueue<PageRelationDto> Queue = new ConcurrentQueue<PageRelationDto>();
        private readonly object _lock = new object();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Stopwatch _stopwatch = new Stopwatch();


        public TaskService(IUnitOfWork unitOfWork, IParserService parserService)
        {
            _unitOfWork = unitOfWork;
            _parserService = parserService;
        }
        public void Execute(bool withExternals, int maxDepth)
        {

            PageRelationDto dto;
            while (true)
            {

                if (Queue.TryDequeue(out dto))
                {
                    Page p;
                    try
                    {
                        var depth = dto?.Depth + 1 ?? 0;
                        if (depth > maxDepth)
                        {
                            continue;
                        }
                        p = _parserService.Parse(dto.Url, withExternals, depth);

                        lock (_lock)
                        {
                            if (dto.ParentUrl == null)
                            {
                                var url = dto.Url;
                                var existingSite = _unitOfWork.SiteRepository.Get(s => s.Url == url).FirstOrDefault();
                                if (existingSite == null)
                                {
                                    Site site = new Site {Url = url};
                                    p.Site = site;
                                }
                                else
                                {
                                    p.Site = existingSite;
                                }
                            }
                            else
                            {
                                var parent = _unitOfWork.PageRepository.Get(dto.ParentUrl);                     
                                p.ParentId = parent.Id;
                                p.Site = parent.Site;
                                if (
                                    !p.Url.Contains(
                                        new Uri(p.Site.Url).GetLeftPart(UriPartial.Authority)
                                            .Replace("www.", "")
                                            .Replace("https://", "")
                                            .Replace("http://", "")))
                                {
                                    p.IsInternal = false;
                                }

                            }

                            var loadedPage = _unitOfWork.PageRepository.Get(p.Url);
                            if (loadedPage != null)
                            {
                                p.Id = loadedPage.Id;
                                // _unitOfWork.PageRepository.Update(p);
                                //_unitOfWork.PageRepository.Add(p);
                                // _unitOfWork.Save();
                            }
                            // else
                            //{
                            var csses = p.Css.ToList();
                            var images = p.Images.ToList();
                            p.Css.Clear();
                            p.Images.Clear();
                            _unitOfWork.PageRepository.Update(p);
                            _unitOfWork.Save();

                            csses.ForEach(c =>
                            {
                                var css = _unitOfWork.CssRepository.Get(c.Link);
                                if (css != null)
                                {
                                    p.Css.Add(css);
                                }
                                else
                                {
                                    _unitOfWork.CssRepository.Add(c);
                                    p.Css.Add(c);
                                }
                            });
                            images.ForEach(i =>
                            {
                                var image = _unitOfWork.ImageRepository.Get(i.Link);
                                if (image != null)
                                {
                                    p.Images.Add(image);
                                }
                                else
                                {
                                    _unitOfWork.ImageRepository.Add(i);
                                    p.Images.Add(i);
                                }
                            });
                            _unitOfWork.PageRepository.Update(p);
                            _unitOfWork.Save();
                        }
                        if (p.Depth != maxDepth)
                        {
                            foreach (var link in p.ChildUlrs)
                            {
                                AddToQueue(link, p);
                            }
                        }

                        // }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("The following Url: " + dto.Url + " returned error message: "+ ex.Message);
                        Console.WriteLine(ex.Message);
                    }

                }
                else
                {
                    Thread.Sleep(2000);
                    if (Queue.IsEmpty)
                    {
                        break;
                    }
                }
            }


        }

        //public void LoadVisitedLinks()
        //{
        //    foreach (var p in _unitOfWork.PageRepository.Get())
        //    {
        //        visitedPages.Add(p.Url);
        //    }
        //}

        public void Run(string url, bool withExternals, int depth, int threadNum = 10)
        {
            //LoadVisitedLinks();
            VisitedPages.Clear();
            AddToQueue(url);

            Logger.Info("Starting Url: "+url +" Execution started");
            _stopwatch.Start();

            var threads = new List<Task>();
            for (int i = 0; i < threadNum; i++)
            {
                var name = i.ToString();
                Task t = Task.Factory.StartNew(() =>
                {
                    Thread.CurrentThread.Name = name;
                    Execute(withExternals, depth);
                });

                threads.Add(t);
            }

            Task.WaitAll(threads.ToArray());

            _stopwatch.Stop();
            Logger.Info("Starting Url: {0} Execution finished, time required: {1} milliseconds",url, _stopwatch.ElapsedMilliseconds);

        }

        public void AddToQueue(string url, Page parent)
        {
            if (!VisitedPages.Contains(url))
            {
                lock (_lock)
                {
                    //if (!visitedPages.Contains(url))
                    //{
                        PageRelationDto dto = new PageRelationDto
                        {
                            Url = url,
                            Depth = parent?.Depth,
                            ParentUrl = parent?.Url
                        };
                        Queue.Enqueue(dto);
                        VisitedPages.Add(url);
                    //}
                }
            }
        }
        public void AddToQueue(string url)
        {
            AddToQueue(url, null);
        }
    }
}
