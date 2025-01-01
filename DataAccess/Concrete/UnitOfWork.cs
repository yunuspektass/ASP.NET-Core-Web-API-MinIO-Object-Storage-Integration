using Core.Entities;
using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly FileManagementDbContext _context;
    private Dictionary<Type, object> _repositories;
    private IDbContextTransaction _transaction;

    public UnitOfWork(FileManagementDbContext context)
    {
        _context = context;
        _repositories = new Dictionary<Type, object>();
    }

    public IRepository<T> GetRepository<T>() where T : BaseEntity
    {
        if (_repositories.ContainsKey(typeof(T)))
        {
            return (IRepository<T>)_repositories[typeof(T)];
        }

        var repository = new DefaultRepository<T>(_context);
        _repositories.Add(typeof(T), repository);
        return repository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
