using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Event:BaseEntity
	{
		public string Title { get; set; }
		public string Desc { get; set; }
		public string ImgUrl { get; set; }
		public List<Tag> Tags { get; set; }
	}
}

