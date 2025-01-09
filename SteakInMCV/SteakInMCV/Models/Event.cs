using System;
namespace SteakInMCV.Models
{
	public class Event:BaseEntity
	{
        public string Title { get; set; }
        public string Desc { get; set; }
        public string ImgUrl { get; set; }
        public string Info { get; set; }
        public List<string> Tags { get; set; }
    }
}

