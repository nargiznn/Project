using System;
using Domain.Common;

namespace Domain.Entities
{
	public class WelcomeImage:BaseEntity
	{
		public bool IsMain { get; set; }
		public string Image { get; set; }
	}
}

