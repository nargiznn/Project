using System;
using SteakInMCV.Models;

namespace SteakInMCV.ViewModels.Events
{
	public class EventVM:BaseEntity
	{
        public string Title { get; set; }
        public string Desc { get; set; }
        public string ImgUrl { get; set; }
        public List<string> TagsName { get; set; }
    }
}

