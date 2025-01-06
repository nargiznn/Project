using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class CuisineRepository : BaseRepository<Cuisine>, ICuisineRepository
    {
        public CuisineRepository(AppDbContext context) : base(context)
        {
        }
    }
}

