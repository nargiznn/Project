using System;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Service.Helpers.DTOs.SocialMediaLink;

namespace Service.Helpers.DTOs.Chef
{
	public class ChefEditDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public SocialMediaLinkEditDto? SocialMedia { get; set; }
        public List<IFormFile>? Photos { get; set; }
    }
}

