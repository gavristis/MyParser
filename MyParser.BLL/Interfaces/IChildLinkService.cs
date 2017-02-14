using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyParser.Models;

namespace MyParser.BLL.Interfaces
{
    public interface IChildLinkService
    {
        void GetInternals(Page link);
        void GetLinks(Page link);
    }
}
