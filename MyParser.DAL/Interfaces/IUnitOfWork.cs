namespace MyParser.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IPageRepository PageRepository { get; }


        void Save();
    }
}
