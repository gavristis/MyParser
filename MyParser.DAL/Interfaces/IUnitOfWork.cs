﻿namespace MyParser.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IPageRepository PageRepository { get; }
        IImageRepository ImageRepository { get; }
        ICssRepository CssRepository { get; }
        ISiteRepository SiteRepository { get; }
        void Save();
    }
}
