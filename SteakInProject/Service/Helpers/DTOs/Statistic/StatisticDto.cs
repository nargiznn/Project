using System;
using System.ComponentModel.DataAnnotations;

namespace Service.Helpers.DTOs.Statistic
{
	public class StatisticDto
	{
        [Required]
        public string Title { get; set; }

        [Required]
        public int Value { get; set; }
    }
}

