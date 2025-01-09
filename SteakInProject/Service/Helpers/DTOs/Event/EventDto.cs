using System;
namespace Service.Helpers.DTOs.Event
{
	public class EventDto
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string ImgUrl { get; set; }
        public string Info { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<string> Tags { get; set; }
    
    }
}

