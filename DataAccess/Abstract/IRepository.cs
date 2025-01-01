using System.Linq.Expressions;
using Core.Entities;

namespace DataAccess.Abstract;

public interface IRepository<T> where T : BaseEntity
{
    Task<List<T>> GetAll();
    Task Add(T item);
    Task AddRange(List<T> list);
    Task Delete(T item);
    Task DeleteRange(List<T> list);
    Task Update(T item);
    Task UpdateRange(List<T> list);
    Task Destroy(T item);
    Task DestroyRange(List<T> list);
    Task<List<T>> Where(Expression<Func<T, bool>> exp);
    Task<bool> Any(Expression<Func<T, bool>> exp);
    Task<T> FirstOrDefault(Expression<Func<T, bool>> exp);
    Task<object> Select(Expression<Func<T, object>> exp);
    Task<IQueryable<X>> Select<X>(Expression<Func<T, X>> exp);
    Task<T?> Find(int id);
}
