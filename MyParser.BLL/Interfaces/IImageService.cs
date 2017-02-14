using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyParser.Models;

namespace MyParser.BLL.Interfaces
{
    public interface IImageService
    {
        void SearchImages(Page link);
    }
}
