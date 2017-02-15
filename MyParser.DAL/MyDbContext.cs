using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using MyParser.Models;

namespace MyParser.DAL
{
    public class MyDbContext : DbContext
    {
        static MyDbContext()
        {
            Database.SetInitializer(new MyDbInitializer());
        }

        public MyDbContext()
            : base("MyConnection")
        {
        }

        public static MyDbContext Create()
        {
            return new MyDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Css> Csses { get; set; }
        public virtual DbSet<Image> Images { get; set; }
    }
}
