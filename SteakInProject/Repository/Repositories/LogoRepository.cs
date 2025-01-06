using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class AwardLogoRepository : BaseRepository<AwardLogo>, IAwardLogoRepository
    {
        public AwardLogoRepository(AppDbContext context) : base(context)
        {
        }
	}
}

