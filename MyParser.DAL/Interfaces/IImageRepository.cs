using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyParser.Models;

namespace MyParser.DAL.Interfaces
{
    public interface IImageRepository
    {
        Image Get(string url);
        void Add(Image image);
        void AttachEntity(Image image);
    }
}
