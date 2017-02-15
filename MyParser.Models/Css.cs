namespace MyParser.Models
{
    public class Css
    {
        public long Id { get; set; }

        public virtual Page Page { get; set; }

        public string Link { get; set; }
    }
}
