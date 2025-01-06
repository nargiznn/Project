using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class WelcomeImageRepository: BaseRepository<WelcomeImage>, IWelcomeImageRepository
    {
        public WelcomeImageRepository(AppDbContext context) : base(context)
        {
        }
    }
}

