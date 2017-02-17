using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        public static HashSet<string> visitedPages = new HashSet<string>();
        public ConcurrentQueue<PageRelationDto> queue = new ConcurrentQueue<PageRelationDto>();
        private readonly object _lock = new object();
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
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
                //Console.WriteLine("Thread #{0} started executing", Thread.CurrentThread.Name);
                if (queue.TryDequeue(out dto))
                {
                    //Console.WriteLine("Thread #{0} has taken url: {1}", Thread.CurrentThread.Name, dto.Url);

                    try
                    {
                        var depth = dto.Parent == null ? 0 : dto.Parent.Depth + 1;
                        if (depth > maxDepth)
                        {
                            continue;
                        }
                        var p = _parserService.Parse(dto.Url, withExternals, depth);
                        p.Parent = dto.Parent;
                        foreach (var link in p.ChildUlrs)
                        {
                            AddToQueue(link, p);
                        }
                        lock (_lock)
                        {
                            //var loadedPage = _unitOfWork.PageRepository.Get(s => s.Url == p.Url).First();
                            //if (loadedPage != null)
                            //{
                            //    _unitOfWork.PageRepository.Delete(loadedPage);
                            //    _unitOfWork.PageRepository.Add(p);
                            //    _unitOfWork.Save();
                            //}


                            _unitOfWork.PageRepository.Add(p);
                            _unitOfWork.Save();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
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

        public void LoadVisitedLinks()
        {
            foreach (var p in _unitOfWork.PageRepository.Get())
            {
                visitedPages.Add(p.Url);
            }
        }

        public void Run(string url, bool withExternals, int depth, int threadNum = 10)
        {
            LoadVisitedLinks();
            AddToQueue(url);

            _logger.Info("Execution started");
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
            _logger.Info("Execution finished, time required: {0} milliseconds", _stopwatch.ElapsedMilliseconds);

        }

        public void AddToQueue(string url, Page parent)
        {
            if (!visitedPages.Contains(url))
            {
                lock (_lock)
                {
                    //if (!visitedPages.Contains(url))  //TODO: possibly remove
                    //{
                        PageRelationDto dto = new PageRelationDto
                        {
                            Url = url,
                            Parent = parent
                        };
                        queue.Enqueue(dto);
                        visitedPages.Add(url);
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
