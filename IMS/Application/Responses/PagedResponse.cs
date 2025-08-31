// Application/Responses/PagedResponse.cs
using System.Collections.Generic;

namespace Application.Responses
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)System.Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        public List<T> Items { get; set; } = new List<T>();

        public PagedResponse()
        {
        }

        public PagedResponse(List<T> items, int totalCount, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}