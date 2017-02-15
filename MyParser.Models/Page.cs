using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HtmlAgilityPack;

namespace MyParser.Models
{
    public class Page
    {
        [Key]
        public long Id { get; set; }

        public string Url { get; set; }

        [NotMapped]
        public Uri Uri
        {
            get
            {
                return new Uri(Url);
            }
        } 

        [NotMapped]
        public HtmlDocument HtmlDocument { get; set ; }

        [NotMapped]
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
