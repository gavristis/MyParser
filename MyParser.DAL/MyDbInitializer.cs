using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyParser.DAL
{
    public class MyDbInitializer : DropCreateDatabaseIfModelChanges<MyDbContext>
    {
        //protected override void Seed(MyDbContext context)
        //{

        //    base.Seed(context);
        //}
    }
}
