using Orcus.Core.DataAccess.RepositoryPattern;
using System;
using System.Data;

namespace Orcus.Core.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        int SaveChanges();
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        bool CommitTransaction();
        void RollbackTransaction();
        void Dispose(bool disposing);
    }
}
