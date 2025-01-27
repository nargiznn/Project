using System;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace Service.Helpers.DTOs.Event
{
	public class EventEditDto
	{
        public string? Title { get; set; }
        public string? Desc { get; set; }
        [SwaggerSchema(ReadOnly = true)]
        public string? ImgUrl { get; set; }
        public IFormFile UploadImage { get; set; }
        public string? Info { get; set; }
        public List<int>? TagIds { get; set; }
    }
}

