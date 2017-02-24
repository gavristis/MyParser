using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MyParser.BLL.Interfaces;
using MyParser.DAL.Interfaces;
using MyParser.Models;
using NLog;

namespace MyParser.BLL.Services
{
    public class SiteTreeService : ISiteTreeService
    {
        private readonly string _path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private readonly IUnitOfWork _unitOfWork;
        private StreamWriter _file;
        public static List<Page> ChildPages = new List<Page>();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public SiteTreeService(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public void BuildTree(Page startingPage, int maxDepth)
        {
            if (startingPage.Depth <= maxDepth)
            {
                ChildPages = startingPage.ChildLinks.ToList();
                var prefix = new string('-', startingPage.Depth);
                using (_file = new StreamWriter(_path + @"\" + new Uri(startingPage.Site.Url).Host +".txt", true))
                {
                    _file.WriteLine("|" + prefix + startingPage.Uri.AbsoluteUri);
                }
                Logger.Info(_path + @"\" + new Uri(startingPage.Site.Url).Host + ".txt");
                foreach (var page in ChildPages)
                {
                    
                    //page.Depth = startingPage.Depth + 1;
                    //Console.WriteLine(page.Url +" page has depth of "+ page.Depth);
                    BuildTree(page, maxDepth);
                }
            }
        }

        public void BuildTree(int maxDepth)
        {
            var sites = _unitOfWork.SiteRepository.Get(s=>s.Url!=null);
            foreach (var site in sites)
            {
                var startingPage = site.Pages.FirstOrDefault(p => p.Depth == 0);
                if (startingPage!=null)
                {
                    BuildTree(startingPage, maxDepth);
                }                
            }
        }
        public void BuildTree(string startingUrl, int maxDepth)
        {
            try
            {
                var startingPage = _unitOfWork.PageRepository.Get(startingUrl);
                Logger.Info("Started building site tree for the following URL: " + startingUrl);
                BuildTree(startingPage, maxDepth);
                Logger.Info("Successfully finished site tree for the following URL: " + startingUrl);
            }
            catch (Exception e)
            {
                Logger.Error("The following URL: " + startingUrl + " returned error " + e.Message);
            }
            
                        
        }
    }
}
