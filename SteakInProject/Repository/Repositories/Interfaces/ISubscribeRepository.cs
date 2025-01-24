using System;
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
	public interface ISubscribeRepository: IBaseRepository<Subscribe>
    {
        Task<List<string>> GetSubscribedEmailsAsync();
        Task SaveChangesAsync();
    }
}


