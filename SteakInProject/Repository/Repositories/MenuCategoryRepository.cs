using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class MenuCategoryRepository: BaseRepository<MenuCategory>, IMenuCategoryRepository
    {
        public MenuCategoryRepository(AppDbContext context) : base(context)
        {
        }
	}
}

