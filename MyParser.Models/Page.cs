using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HtmlAgilityPack;

namespace MyParser.Models
{
    public class Page
    {
        private Uri _uri;

        [Key]
        public long Id { get; set; }

        [Index("IX_Url", IsUnique = true)]
        [MaxLength(450)]
        public string Url { get; set; }

        [NotMapped]
        public Uri Uri
        {
            get
            {
                if (_uri == null)
                {
                    _uri = new Uri(Url);
                }

                return _uri;
            }
        }

        [NotMapped]
        public HtmlDocument HtmlDocument { get; set ; }

        [NotMapped]
        public virtual ICollection<string> ChildUlrs { get; set; }

        public virtual ICollection<Page> ChildLinks { get; set; }

        public virtual ICollection<Css> Css { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        //public long SiteId { get; set; }

        public virtual  Site Site { get; set; }

        public long LoadTime { get; set; }

        public long Size { get; set; }

        public bool IsInternal { get; set; } = true;

        public long? ParentId { get; set; }        

        public virtual Page Parent { get; set; }

        public int Depth { get; set; }

    }
}
