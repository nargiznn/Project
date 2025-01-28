using System;
using System.Linq.Expressions;
using Domain.Common;

namespace Repository.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task CreateAsync(T entity);
        Task EditAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllWithExpression(Expression<Func<T, bool>> predicate);
        Task<T> GetWithExpression(Expression<Func<T, bool>> predicate);
        Task DeleteAsync(int id);
        Task<bool> IsExist(Expression<Func<T, bool>> expression);

    }
}

