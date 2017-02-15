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
        private string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private readonly IPageService pageService;
        private readonly IUnitOfWork _unitOfWork;
        private StreamWriter file;
        public static List<Page> childPages = new List<Page>();

        public SiteTreeService(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public void BuildTree(Page startingPage, int maxDepth)
        {
            if (startingPage.Depth <= maxDepth)
            {
                childPages = startingPage.ChildLinks.ToList();
                var prefix = new string('-', startingPage.Depth);
                string line = startingPage.Depth + startingPage.Uri.AbsoluteUri;
                Console.WriteLine("|" + prefix + line);
                using (file = new StreamWriter(path + @"\" + startingPage.Uri.Host + ".txt", true))
                {
                    file.WriteLine("|" + prefix + line);
                }
                
                foreach (var page in childPages)
                {
                    
                    //page.Depth = startingPage.Depth + 1;
                    //Console.WriteLine(page.Url +" page has depth of "+ page.Depth);
                    BuildTree(page, maxDepth);
                }
            }
        }
        public void BuildTree(string startingUrl, int maxDepth)
        {
            Page startingPage = _unitOfWork.PageRepository.Get(s => s.Uri.AbsoluteUri == startingUrl).First();            
            
            //startingPage.Depth = 0;
            BuildTree(startingPage, maxDepth);
                        
        }
    }
}
