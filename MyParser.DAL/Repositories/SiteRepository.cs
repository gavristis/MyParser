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

        public void Add(Site site)
        {
            _db.Sites.Add(site);
        }
    }
}
