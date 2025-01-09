using System;
namespace Service.Helpers.DTOs.Event
{
	public class EventEditDto
	{
        public string? Title { get; set; }
        public string? Desc { get; set; }
        public string? ImgUrl { get; set; }
        public string? Info { get; set; }
        public List<int>? TagIds { get; set; }
    }
}

