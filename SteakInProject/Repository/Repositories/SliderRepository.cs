using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;
namespace Repository.Repositories
{
	public class SliderRepository : BaseRepository<Slider>, ISliderRepository
    {
        public SliderRepository(AppDbContext context) : base(context)
        {
        }
    }
}

