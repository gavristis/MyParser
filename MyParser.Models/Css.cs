using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyParser.Models
{
    public class Css
    {
        public long Id { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
        [Index("IX_Css_Link", IsUnique = true)]
        [MaxLength(450)]
        public string Link { get; set; }
    }
}
