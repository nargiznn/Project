using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Slider:BaseEntity
	{
		public string Title { get; set; }
        public string MainTitle { get; set; }
        public string Desc { get; set; }
        public string BtnText { get; set; }
        public string Image { get; set; }
    }
}

