using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;
namespace Repository.Repositories
{
    public class WelcomeInfoRepository : BaseRepository<WelcomeInfo>, IWelcomeInfoRepository
    {
        public WelcomeInfoRepository(AppDbContext context) : base(context)
        {
        }
    }
}

