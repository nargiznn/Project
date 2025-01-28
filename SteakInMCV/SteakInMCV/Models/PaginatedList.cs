using System;
namespace SteakInMCV.Models
{
	public class PaginatedList<T>
	{
        public PaginatedList(List<T> items, int totalPages, int pageIndex, int pageSize)
        {
            this.Items = items;
            this.TotalPages = totalPages;
            this.PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrev => PageIndex > 1;
        public bool HasNext => PageIndex < TotalPages;

        public static PaginatedList<T> Create(IQueryable<T> query, int pageIndex, int pageSize)
        {
            int totalPages = (int)Math.Ceiling(query.Count() / (double)pageSize);
            var items = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, totalPages, pageIndex, pageSize);
        }
    }
}

