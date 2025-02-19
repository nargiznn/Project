using System;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace Service.Helpers.DTOs.Event
{
	public class EventEditDto
	{
        public string? Title { get; set; }
        public string? Desc { get; set; }
        public IFormFile? Image { get; set; }
        public string? Info { get; set; }
        public List<int>? TagIds { get; set; }
    }
}

