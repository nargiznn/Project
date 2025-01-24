using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
	public class ChefImage
	{
        public int Id { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public string Image { get; set; }
        public string Path { get; set; }
        public int ChefId { get; set; }
        public Chef Chef { get; set; }


    }
}

