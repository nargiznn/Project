using System;
using Domain.Entities;

namespace Repository.Repositories.Interfaces
{
	public interface IMealPackageRepository: IBaseRepository<MealPackage>
    {
        Task<IEnumerable<MealPackage>> GetAllWithIncludeAsync(
           Func<IQueryable<MealPackage>, IQueryable<MealPackage>> include = null);
    }

}

