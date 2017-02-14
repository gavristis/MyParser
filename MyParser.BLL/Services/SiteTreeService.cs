using System;
using System.Collections.Generic;
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
        private IPageService pageService;
        private IUnitOfWork _unitOfWork;
        public SiteTreeService(IUnitOfWork unit)
        {
            _unitOfWork = unit;
        }
        public void BuildTree(Page startingPage, int maxDepth)
        {
            if (startingPage.Depth < maxDepth)
            {
                Console.WriteLine(startingPage.Depth + startingPage.Uri.AbsoluteUri);
                childPages = startingPage.ChildLinks.ToList();
                //childPages = _unitOfWork.PageRepository.Get().Where(s => s.Parent == startingPage).ToList();
                foreach (var page in childPages)
                {
                    BuildTree(page, maxDepth);
                }
            }
        }
        public void BuildTree(string startingUrl, int maxDepth)
        {
            Page startingPage = _unitOfWork.PageRepository.Get(s => s.Uri.AbsoluteUri == startingUrl).First();
            
            BuildTree(startingPage, maxDepth);
                        
        }
    }
}
