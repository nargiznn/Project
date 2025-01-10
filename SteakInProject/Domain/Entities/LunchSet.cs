using System;
using Domain.Common;

namespace Domain.Entities
{
	public class LunchSet:BaseEntity
	{
        public string Title { get; set; }
        public string Desc { get; set; }
        public decimal Price { get; set; }
        public ICollection<LunchSetProduct> LunchSetProducts { get; set; }

    }
}

