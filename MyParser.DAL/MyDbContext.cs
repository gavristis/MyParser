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
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


            //modelBuilder.Entity<Page>()
            //    .HasMany(f => f.Css)
            //    .WithRequired(p => p.Page)
            //    .WillCascadeOnDelete();
            //modelBuilder.Entity<Page>()
            //    .HasMany(f => f.Images)
            //    .WithRequired(p => p.Page)
            //    .WillCascadeOnDelete();
            //modelBuilder.Entity<Css>()
            //    .HasKey(p => p.Id)
            //    .HasRequired(p => p.Page)
            //    .WithOptional();
            //modelBuilder.Entity<Image>()
            //    .HasKey(p => p.Id)
            //    .HasRequired(p => p.Page)
            //    .WithOptional();
        }

        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Css> Csses { get; set; }
        public virtual DbSet<Image> Images { get; set; }
    }
}
