using System.ComponentModel;
using System.Linq.Expressions;
using Core.Entities;
using DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete;

public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly FileManagementDbContext _db;

    protected BaseRepository(FileManagementDbContext db)
    {
        _db = db;
    }


    public async Task<List<T>> GetAll()
    {
        return await _db.Set<T>().ToListAsync();
    }

    public async Task Add(T item)
    {
        item.CreatedBy = GetCurrentUser();
        item.UpdatedBy = GetCurrentUser();
        await _db.Set<T>().AddAsync(item);
        await _db.SaveChangesAsync();

    }

    public async Task AddRange(List<T> list)
    {
        foreach (var item in list)
        {
            item.CreatedBy = GetCurrentUser();
            item.UpdatedBy = GetCurrentUser();
        }

        await _db.Set<T>().AddRangeAsync(list);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(T item)
    {
        item.Deleted = true;
        item.UpdatedBy = GetCurrentUser();
        await _db.SaveChangesAsync();
    }

    public async Task DeleteRange(List<T> list)
    {
        foreach (T item in list)
        {
            await Delete(item);
        }
    }

    public async Task Update(T item)
    {
        item.UpdatedBy = GetCurrentUser();
        _db.Entry(item).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }

    public async Task UpdateRange(List<T> list)
    {
        foreach (T item in list)
        {
            await Update(item);
        }
    }

    public async Task Destroy(T item)
    {
        _db.Set<T>().RemoveRange(item);
        await _db.SaveChangesAsync();
    }

    public async Task DestroyRange(List<T> list)
    {
        _db.Set<T>().RemoveRange(list);
        await _db.SaveChangesAsync();
    }

    public async Task<List<T>> Where(Expression<Func<T, bool>> exp)
    {
        return await _db.Set<T>().Where(exp).ToListAsync();
    }

    public async Task<bool> Any(Expression<Func<T, bool>> exp)
    {
        return await _db.Set<T>().AnyAsync(exp);
    }

    public async Task<T> FirstOrDefault(Expression<Func<T, bool>> exp)
    {
        return await _db.Set<T>().FirstOrDefaultAsync(exp);
    }

    public async Task<object> Select(Expression<Func<T, object>> exp)
    {
        return await _db.Set<T>().Select(exp).FirstOrDefaultAsync();
    }

    public async Task<IQueryable<X>> Select<X>(Expression<Func<T, X>> exp)
    {
        return _db.Set<T>().Select(exp);
    }


    public async Task<T?> Find(int id)
    {
        return await _db.Set<T>().FindAsync(id);
    }

    private string GetCurrentUser()
    {
        return "Admin";
    }
}
