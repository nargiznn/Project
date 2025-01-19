using System;
using Domain.Entities;
using Repository.Data;
using Repository.Repositories.Interfaces;

namespace Repository.Repositories
{
	public class TestimonialRepository : BaseRepository<Testimonial>, ITestimonialRepository
    {
        public TestimonialRepository(AppDbContext context) : base(context)
    {
    }
	}
}

