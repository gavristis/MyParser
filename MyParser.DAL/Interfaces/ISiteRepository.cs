using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MyParser.Models;

namespace MyParser.DAL.Interfaces
{
    public interface ISiteRepository
    {
        void Add(Site site);
    }
}
