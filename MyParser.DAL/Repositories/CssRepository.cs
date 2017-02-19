using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyParser.DAL.Interfaces;
using MyParser.Models;

namespace MyParser.DAL.Repositories
{
    public class CssRepository : ICssRepository
    {
        private readonly MyDbContext _db;

        public CssRepository(MyDbContext db)
        {
            _db = db;
        }

        public Css Get(string url)
        {
            return _db.Csses.FirstOrDefault(c => c.Link == url);
        }

        public void Add(Css css)
        {
            _db.Csses.Add(css);
        }

        public void AttachEntity(Css css)
        {
            _db.Csses.Attach(css);
        }
    }
}
