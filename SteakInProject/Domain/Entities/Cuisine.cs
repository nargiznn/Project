using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Cuisine:BaseEntity
	{
		public string Name { get; set; }
        public string Desc { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}

