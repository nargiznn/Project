using System;
namespace Service.Helpers.DTOs.Faq
{
	public class FaqEditDto
	{
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public bool? IsActive { get; set; }
    }
}

