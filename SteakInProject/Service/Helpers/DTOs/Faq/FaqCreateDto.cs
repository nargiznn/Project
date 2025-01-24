using System;
namespace Service.Helpers.DTOs.Faq
{
	public class FaqCreateDto
	{
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool? IsActive { get; set; }
    }
}

