using System;
namespace Domain.Entities
{
	public class MealPackageProduct
	{
        public int MealPackageId { get; set; }
        public MealPackage MealPackage { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}

