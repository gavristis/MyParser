﻿using System;
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

        public PageService()
        {
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
            link.Size = Encoding.Unicode.GetByteCount(link.HtmlDocument.DocumentNode.OuterHtml);
            //Console.WriteLine("Page has size of {0} bytes", link.Size);
        }



    }
}
