using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MyParser.Models;

namespace MyParser.DAL.Interfaces
{
    public interface IPageRepository
    {
        void Add(Page item);
        IEnumerable<Page> Get();
        Page Get(long id);
        IQueryable<Page> Get(Expression<Func<Page, bool>> predicate);
        Page Get(string url);
        void Delete(Page item);
        void Update(Page item);
    }
}
