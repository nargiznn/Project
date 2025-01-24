using System;
using Domain.Common;

namespace Service.Helpers.Faqs
{
	public class FaqDto
	{
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

