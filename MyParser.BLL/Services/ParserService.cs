﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MyParser.Models;
using MyParser.BLL.Interfaces;

namespace MyParser.BLL.Services
{
    public class ParserService : IParserService
    {
        private readonly IPageService _pageService;
        private readonly ICssService _cssService;
        private readonly IImageService _imageService;
        private readonly IChildLinkService _childLinkService;
        public ParserService(IPageService pageService, ICssService cssService, IImageService imageService, IChildLinkService childLinkService)
        {
            _pageService = pageService;
            _cssService = cssService;
            _imageService = imageService;
            _childLinkService = childLinkService;
        }
        public Page Parse(string url, bool withExternals, int depth)
        {
            Page page = new Page();
            page.HtmlDocument = new HtmlDocument();
            page.Css = new List<Css>();
            page.Images = new List<Image>();
            page.ChildUlrs = new List<string>();
            page.Url = url;
            _pageService.MeasureTime(page);
            //page.Html = page.HtmlDocument.DocumentNode.OuterHtml;                        
            _pageService.MeasureSize(page);
            _cssService.SearchCss(page);
            _imageService.SearchImages(page);
            page.Depth = depth;
            if (withExternals == false)
            {
                _childLinkService.GetInternals(page);
            }
            else
            {
                _childLinkService.GetLinks(page);
            }
            return page;
        }
    }
}
