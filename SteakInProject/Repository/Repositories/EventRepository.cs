using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class EventRepository:BaseRepository<Event>, IEventRepository
    {
        public EventRepository(AppDbContext context) : base(context)
    {
    }
}
}

