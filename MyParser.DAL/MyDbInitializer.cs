using System.Data.Entity;

namespace MyParser.DAL
{
    public class MyDbInitializer : DropCreateDatabaseIfModelChanges<MyDbContext>
    {

    }
}
