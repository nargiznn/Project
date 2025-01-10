using System;
using Domain.Common;

namespace Domain.Entities
{
	public class MealPackage:BaseEntity
	{
		public string Title { get; set; }
        public string Desc { get; set; }
        public int NumberOfPeople { get; set; } 
        public decimal Price { get; set; }

        public ICollection<MealPackageProduct> MealPackageProducts { get; set; }


    }
}

