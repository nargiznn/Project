using System;
using Domain.Common;

namespace Service.Helpers.DTOs.Award
{
	public class AwardDto:BaseEntity
	{
        public string Name { get; set; }
        public string Year { get; set; }

    }
}

