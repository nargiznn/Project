using System;
namespace SteakInMCV.Models
{
	public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrev => PageIndex > 1;
        public bool HasNext => PageIndex < TotalPages;
    }
}

