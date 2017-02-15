using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MyParser.DAL.Interfaces;
using MyParser.Models;

namespace MyParser.DAL.Repositories
{
    class PageRepository : IPageRepository
    {
        private readonly MyDbContext _db;

        public PageRepository(MyDbContext db)
        {
            _db = db;
        }

        public void Add(Page item)
        {
            _db.Pages.Add(item);
        }

        public void Update(Page item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(Page item)
        {
            _db.Pages.Remove(item);
        }

        public void Delete(long id)
        {
            var page = Get(id);
            Delete(page);
        }

        public IEnumerable<Page> Get()
        {
            return _db.Pages;
        }

        public Page Get(long id)
        {
            return _db.Pages.Find(id);
        }

        public IEnumerable<Page> Get(Func<Page, bool> predicate)
        {
            return _db.Pages.Where(predicate);
        }
    }
}
