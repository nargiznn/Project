using System;
namespace Service.Helpers.DTOs.LunchSet
{
	public class LunchSetDto
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public decimal Price { get; set; }
        public List<string> ProductNames { get; set; }
    }
}

