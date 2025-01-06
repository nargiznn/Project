using System;
using Domain.Common;

namespace Domain.Entities
{
	public class WelcomeInfo:BaseEntity
	{
        public string Title { get; set; }
        public string MainTitle { get; set; }
        public string Icon { get; set; }
        public string Desc { get; set; }
        public string BtnText { get; set; }
    }
}

