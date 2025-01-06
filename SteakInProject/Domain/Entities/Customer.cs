using System;
using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities
{
	public class Customer:BaseEntity
	{
		public string Name { get; set; }
		public string SurName { get; set; }
		public string Image { get; set; }
		public string Text { get; set; }
		public bool IsPermit { get; set; }
        public ReviewType? ReviewType { get; set; }

        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public byte Raiting { get; set; }
    }
}

