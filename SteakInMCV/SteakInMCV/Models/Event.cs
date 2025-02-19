using System;
namespace SteakInMCV.Models
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Title { get; set; }
        public string Desc { get; set; }
        public string ImgUrl { get; set; }
        public string Info { get; set; }
        public List<string> Tags { get; set; }
    }
}

