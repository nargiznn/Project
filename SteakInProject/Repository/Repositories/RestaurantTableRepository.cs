using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class RestaurantTableRepository: BaseRepository<RestaurantTable>, IRestaurantTableRepository
    {
        public RestaurantTableRepository(AppDbContext context) : base(context)
        {
        }
    }
}

