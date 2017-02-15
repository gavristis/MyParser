namespace MyParser.Models
{
    public class Image
    {
        public long Id { get; set; }

        public virtual Page Page { get; set; }

        public string Link { get; set; }
    }
}
