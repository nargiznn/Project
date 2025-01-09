using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.Helpers.DTOs.Client;
using Service.Helpers.DTOs.GalleryCategory;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class ClientService:IClientService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ClientService(AppDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientDto>> GetAllAsync()
        {
            return _mapper.Map<List<ClientDto>>(await _context.Clients.AsNoTracking().ToListAsync());
        }
    }
}

