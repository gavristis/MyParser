using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MyParser.BLL.Interfaces;
using MyParser.DAL.Interfaces;
using MyParser.Models;

namespace MyParser.BLL.Services
{
    public class SiteTreeService : ISiteTreeService
    {
        public static List<Page> childPages = new List<Page>();
        private readonly IPageService pageService;
        private readonly IUnitOfWork _unitOfWork;
        private StreamWriter file;
        public SiteTreeService(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public void BuildTree(Page startingPage, int maxDepth)
        {
            if (startingPage.Depth <= maxDepth)
            {
                var prefix = new string('-', startingPage.Depth);
                string line = startingPage.Depth + startingPage.Uri.AbsoluteUri;
                file.WriteLine("|"+prefix + line);
                childPages = startingPage.ChildLinks.ToList();
                foreach (var page in childPages)
                {
                    Console.WriteLine(page.Url);
                    page.Depth = startingPage.Depth + 1;
                    BuildTree(page, maxDepth);
                }
            }
        }
        public void BuildTree(string startingUrl, int maxDepth)
        {
            Page startingPage = _unitOfWork.PageRepository.Get(s => s.Uri.AbsoluteUri == startingUrl).First();
            file = new StreamWriter(@"C:\Users\vgavrilov\Desktop\SiteTree.txt");
            startingPage.Depth = 0;
            BuildTree(startingPage, maxDepth);
                        
        }
    }
}
