using System;
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
    public interface IMenuCategoryRepository : IBaseRepository<MenuCategory>
    {
        Task<IEnumerable<MenuCategory>> GetAllWithIncludeAsync(
            Func<IQueryable<MenuCategory>, IQueryable<MenuCategory>> include = null);
    }
}

