using Core.Entities;

namespace DataAccess.Abstract;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> GetRepository<T>() where T : BaseEntity;
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
