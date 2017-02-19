using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyParser.Models;

namespace MyParser.DAL.Interfaces
{
    public interface ICssRepository
    {
        Css Get(string url);
        void Add(Css css);
        void AttachEntity(Css css);
    }
}
