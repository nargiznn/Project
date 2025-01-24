using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
    public class MenuCategoryRepository : BaseRepository<MenuCategory>, IMenuCategoryRepository
    {
        public MenuCategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MenuCategory>> GetAllWithIncludeAsync(
            Func<IQueryable<MenuCategory>, IQueryable<MenuCategory>> include = null)
        {
            IQueryable<MenuCategory> query = _context.Set<MenuCategory>();

            if (include != null)
            {
                query = include(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }
    }
}
