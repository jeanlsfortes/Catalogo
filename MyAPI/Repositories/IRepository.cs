using MyAPI.Models;
using MyAPI.Pagination;
using System.Linq.Expressions;
using X.PagedList;

namespace MyAPI.Repositories;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
}
