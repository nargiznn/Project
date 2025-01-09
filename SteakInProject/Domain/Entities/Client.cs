using System;
using Domain.Common;

namespace Domain.Entities
{
	public class Client:BaseEntity
	{
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}

