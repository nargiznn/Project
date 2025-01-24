using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class LunchSetRepository: BaseRepository<LunchSet>, ILunchSetRepository
    {
        private readonly AppDbContext _context;

        public LunchSetRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LunchSet>> GetAllWithIncludeAsync(
            Func<IQueryable<LunchSet>, IQueryable<LunchSet>> include = null)
        {
            IQueryable<LunchSet> query = _context.LunchSets;

            if (include != null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }
    }
}

