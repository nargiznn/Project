using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class SpecialCategoryRepository : BaseRepository<SpecialCategory>, ISpecialCategoryRepository
    {
        public SpecialCategoryRepository(AppDbContext context) : base(context)
        {
        }
	}
}

