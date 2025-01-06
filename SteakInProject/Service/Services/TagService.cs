using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Service.Helpers.DTOs.Tag;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class TagService:ITagService
	{
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TagService(AppDbContext context,
                              IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateAsync(TagCreateDto tag)
        {
            await _context.Tags.AddAsync(_mapper.Map<Tag>(tag));
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id) ?? throw new NotFoundException("Data notfound");
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(int id, TagEditDto tag)
        {
            var existTag = await _context.Tags.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id) ?? throw new NotFoundException("Data notfound");

            _mapper.Map(tag, existTag);

            _context.Tags.Update(existTag);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TagDto>> GetAllAsync()
        {
            return _mapper.Map<List<TagDto>>(await _context.Tags.AsNoTracking().ToListAsync());
        }

        public async Task<TagDto> GetByIdAsync(int id)
        {
            var result = await _context.Tags.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (result is null) return null;

            return _mapper.Map<TagDto>(result);
        }
    }
}

