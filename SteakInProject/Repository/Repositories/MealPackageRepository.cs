using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class MealPackageRepository: BaseRepository<MealPackage>, IMealPackageRepository
    {
        private readonly AppDbContext _context;

        public MealPackageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MealPackage>> GetAllWithIncludeAsync(
            Func<IQueryable<MealPackage>, IQueryable<MealPackage>> include = null)
        {
            IQueryable<MealPackage> query = _context.MealPackages;

            if (include != null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }
    }
}

