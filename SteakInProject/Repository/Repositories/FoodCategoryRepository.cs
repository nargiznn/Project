using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class FoodCategoryRepository:BaseRepository<FoodCategory>, IFoodCategoryRepository
    {
        public FoodCategoryRepository(AppDbContext context) : base(context)
    {
    }
	}
}

