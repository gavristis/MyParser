using System;
using System.Collections.Generic;
using MyParser.Models;

namespace MyParser.DAL.Interfaces
{
    public interface ISiteRepository
    {
        IEnumerable<Site> Get(Func<Site, bool> predicate);
        IEnumerable<Site> Get();
    }
}
