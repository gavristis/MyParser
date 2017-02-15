using MyParser.DAL.Interfaces;

namespace MyParser.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IPageRepository _pageRepository;

        private readonly MyDbContext _db;

        public UnitOfWork()
        {
            _db = MyDbContext.Create();
        }

        public IPageRepository PageRepository
        {
            get { return _pageRepository ?? (_pageRepository = new PageRepository(_db)); }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
