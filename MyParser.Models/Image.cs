using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyParser.Models
{
    public class Image
    {
        public long Id { get; set; }

        public virtual ICollection<Page> Pages { get; set; }

       /* [Index("IX_Image_Link", IsUnique = true)]
        [MaxLength(450)]*/
        public string Link { get; set; }
    }
}
