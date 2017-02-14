using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyParser.Models
{
    public class Css
    {
        public long Id { get; set; }

        public virtual Page Page { get; set; }

        public string Link { get; set; }
    }
}
