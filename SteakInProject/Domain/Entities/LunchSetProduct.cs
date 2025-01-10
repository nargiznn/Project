using System;
using Domain.Common;

namespace Domain.Entities
{
	public class LunchSetProduct
	{
        public int LunchSetId { get; set; }
        public LunchSet LunchSet { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}

