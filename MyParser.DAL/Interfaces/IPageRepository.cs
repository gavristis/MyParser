using System;
using System.Collections.Generic;
using MyParser.Models;

namespace MyParser.DAL.Interfaces
{
    public interface IPageRepository
    {
        void Add(Page item);
        IEnumerable<Page> Get();
        Page Get(long id);
        IEnumerable<Page> Get(Func<Page, bool> predicate);
        void Delete(Page item);
        void Update(Page item);
    }
}
