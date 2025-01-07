using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Faq:BaseEntity
	{
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

