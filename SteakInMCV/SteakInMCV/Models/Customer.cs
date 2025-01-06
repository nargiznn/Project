using System;
using System.ComponentModel.DataAnnotations;

namespace SteakInMCV.Models
{
	public class Customer:BaseEntity
	{
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Image { get; set; }
        public string Text { get; set; }
        public byte Raiting { get; set; }
        public bool IsPermit { get; set; }
        public ReviewType? ReviewType { get; set; }


    }
}


