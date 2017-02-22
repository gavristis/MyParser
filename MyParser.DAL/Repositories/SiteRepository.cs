using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyParser.DAL.Interfaces;
using MyParser.Models;

namespace MyParser.DAL.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly MyDbContext _db;

        public SiteRepository(MyDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Site> Get(Func<Site, bool> predicate)
        {
            return _db.Sites.Where(predicate);
        }
        public IEnumerable<Site> Get()
        {
            return _db.Sites;
        }
    }
}
