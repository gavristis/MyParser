using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyParser.Models
{
    public class Site
    {
        [Key]
        public long Id { get; set; }
        [Index("IX_Site_Url", IsUnique = true)]
        [MaxLength(450)]
        public string Url { get; set; }
        public virtual ICollection<Page> Pages { get; set; }

    }
}
