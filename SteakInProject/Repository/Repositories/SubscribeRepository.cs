using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class SubscribeRepository: BaseRepository<Subscribe>, ISubscribeRepository
    {
        public SubscribeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<string>> GetSubscribedEmailsAsync() =>
             await _context.Subscribes
            .Select(s => s.Email)
            .ToListAsync();

                    //.Where(s => !s.SoftDeleted)

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

