using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyParser.DAL.Interfaces;
using MyParser.Models;

namespace MyParser.DAL.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly MyDbContext _db;

        public ImageRepository(MyDbContext db)
        {
            _db = db;
        }

        public Image Get(string url)
        {
            return _db.Images.FirstOrDefault(i => i.Link == url);
        }

        public void Add(Image image)
        {
            _db.Images.Add(image);
        }

        public void AttachEntity(Image image)
        {
            _db.Images.Attach(image);
        }
    }
}
