using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MyParser.Models;

namespace MyParser.BLL.Models
{
    public class PageDto
    {
        public long Id { get; set; }

        public string Url { get; set; }

        public Uri Uri
        {
            get
            {
                return new Uri(Url);
            }
        }

        public HtmlDocument HtmlDocument { get; set; }

        public virtual ICollection<string> ChildUlrs { get; set; }

        public virtual ICollection<Page> ChildLinks { get; set; }

        public virtual ICollection<Css> Css { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public long LoadTime { get; set; }

        public long Size { get; set; }

        //public bool Downloaded { get; set; }

        public virtual Page Parent { get; set; }

        public int Depth { get; set; }

    }
}
