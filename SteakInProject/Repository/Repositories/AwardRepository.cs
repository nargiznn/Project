using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class AwardRepository: BaseRepository<Award>, IAwardRepository
    {
        public AwardRepository(AppDbContext context) : base(context)
        {
        }
	}
}

