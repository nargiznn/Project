using System;
using Domain.Common;

namespace Domain.Entities
{
	public class ProductCuisine:BaseEntity
	{
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CuisineId { get; set; }
        public Cuisine Cuisine { get; set; }
        public double Percentage { get; set; }
    }
}

