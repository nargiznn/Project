using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
	public class Statistic
	{
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int Value { get; set; }
    }
}

