using System;
using SteakInMCV.Models;

namespace SteakInMCV.ViewModels.Events
{
	public class EventVM
	{
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Title { get; set; }
        public string Desc { get; set; }
        public string ImgUrl { get; set; }
        public string Info { get; set; }
        public List<string> TagsName { get; set; }
    }
}

