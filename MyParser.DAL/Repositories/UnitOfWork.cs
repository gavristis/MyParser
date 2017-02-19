using MyParser.DAL.Interfaces;

namespace MyParser.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IPageRepository _pageRepository;
        private IImageRepository _imageRepository;
        private ICssRepository _cssRepository;

        private readonly MyDbContext _db;

        public UnitOfWork()
        {
            _db = MyDbContext.Create();
        }

        public IPageRepository PageRepository
        {
            get { return _pageRepository ?? (_pageRepository = new PageRepository(_db)); }
        }
        public IImageRepository ImageRepository
        {
            get { return _imageRepository ?? (_imageRepository = new ImageRepository(_db)); }
        }
        public ICssRepository CssRepository
        {
            get { return _cssRepository ?? (_cssRepository = new CssRepository(_db)); }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
