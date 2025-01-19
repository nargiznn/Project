using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.Table;
using Service.Helpers.Faqs;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class RestaurantTableService : IRestaurantTableService
    {
        private readonly IFaqRepository _faqRepo;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RestaurantTableService(AppDbContext context,
                              IMapper mapper,
                              IFaqRepository faqRepository)
        {
            _context = context;
            _mapper = mapper;
            _faqRepo = faqRepository;
        }

        public async Task<IEnumerable<RestaurantTableDto>> GetAllAsync()
        {
            return _mapper.Map<List<RestaurantTableDto>>(await _context.RestaurantTables.AsNoTracking().ToListAsync());
        }


    }
}

