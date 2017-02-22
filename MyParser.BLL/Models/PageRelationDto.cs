using MyParser.Models;

namespace MyParser.BLL.Models
{
    public class PageRelationDto
    {
        public int? Depth { get; set; }

        public string ParentUrl { get; set; }

        public string Url { get; set; }
    }
}
