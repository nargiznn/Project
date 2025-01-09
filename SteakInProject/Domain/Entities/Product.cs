using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Product:BaseEntity
	{
		public string Name { get; set; }
		public string Ingredient { get; set; }
		public double Price { get; set; }
        public int SalesCount { get; set; }
        public int MenuCategoryId { get; set; }
        public MenuCategory MenuCategory { get; set; }
        public int? SpecialCategoryId { get; set; }
        public SpecialCategory? SpecialCategory { get; set; }
        public int FoodCategoryId { get; set; }
        public FoodCategory FoodCategory { get; set; }
        public int CuisineId { get; set; }
        public Cuisine Cuisine { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }

    }
}

