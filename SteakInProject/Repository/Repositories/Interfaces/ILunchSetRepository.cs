using System;
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
	public interface ILunchSetRepository: IBaseRepository<LunchSet>
    {
        Task<IEnumerable<LunchSet>> GetAllWithIncludeAsync(
           Func<IQueryable<LunchSet>, IQueryable<LunchSet>> include = null);
    }
}

