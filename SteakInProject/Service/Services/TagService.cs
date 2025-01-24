using System;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Exceptions;
using Repository.Repositories.Interfaces;
using Service.Helpers.DTOs.Tag;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class TagService:ITagService
	{
        private readonly ITagRepository _tagRepo;
        private readonly IMapper _mapper;
        public TagService(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepo = tagRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(TagCreateDto tag)
        {
            var existingTag = await _tagRepo.GetAllWithExpression(
                x => x.Name == tag.Name 
            );
            if (existingTag.Any())
            {
                throw new ArgumentException("An Tag with the same name already exists.");
            }

            await _tagRepo.CreateAsync(_mapper.Map<Tag>(tag));
        }
        public async Task DeleteAsync(int id)
        {
            await _tagRepo.DeleteAsync(id);
        }

        public async Task<IEnumerable<TagDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<TagDto>>(await _tagRepo.GetAllAsync());
        }

        public async Task<TagDto> GetByIdAsync(int id)
        {
            return _mapper.Map<TagDto>(await _tagRepo.GetByIdAsync(id));
        }

        public async Task<IEnumerable<TagDto>> SearchAsync(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                var allTags = await _tagRepo.GetAllAsync();
                return _mapper.Map<IEnumerable<TagDto>>(allTags);
            }
            var tags = await _tagRepo.GetAllWithExpression(c =>
                c.Name.Contains(str)
            );

            if (!tags.Any())
            {
                throw new NotFoundException("No tags found matching the search criteria.");
            }

            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }


        public async Task EditAsync(int id, TagEditDto tag)
        {
            var existingTag = await _tagRepo.GetByIdAsync(id);
            if (existingTag == null)
            {
                throw new NotFoundException("Tag not found");
            }
            var duplicateTag = await _tagRepo.GetAllWithExpression(
                x => x.Name == (tag.Name ?? existingTag.Name) &&
                     x.Id != id
            );


            if (duplicateTag.Any())
            {
                throw new ArgumentException("An tag with the same name already exists.");
            }

            existingTag.Name = string.IsNullOrWhiteSpace(tag.Name) ? existingTag.Name : tag.Name;

            await _tagRepo.EditAsync(existingTag);
        }
    }
}

