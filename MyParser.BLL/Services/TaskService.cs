using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyParser.BLL.Interfaces;
using MyParser.BLL.Models;
using MyParser.DAL.Interfaces;
using MyParser.Models;

namespace MyParser.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IParserService _parserService;
        public static List<string> visitedPages = new List<string>();//to task service
        public ConcurrentQueue<PageRelationDto> queue = new ConcurrentQueue<PageRelationDto>();
        private readonly object _lock = new object();

        public TaskService(IUnitOfWork unitOfWork, IParserService parserService)
        {
            _unitOfWork = unitOfWork;
            _parserService = parserService;
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
                        var p = _parserService.Parse(dto.Url, withExternals, depth);
                        p.Parent = dto.Parent;
                        lock (_lock)
                        {
                            _unitOfWork.PageRepository.Add(p);
                            _unitOfWork.Save();
                        }
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
        public void Run(int threadNum, bool withExternals, int depth)// to task service
        {
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
        public void AddToQueue(string url)
        {
            AddToQueue(url, null);
        }
    }
}
