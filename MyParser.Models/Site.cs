using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyParser.Models
{
    public class Site
    {
        [Key]
        public long Id { get; set; }
        public string Url { get; set; }
        public virtual ICollection<Page> Pages { get; set; }

    }
}
