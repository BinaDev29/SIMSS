namespace Application.DTOs.Common
{
    public class PagedRequestDto
    {
        private int _pageNumber = 1;
        private int _pageSize = 10;

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value > 0 ? value : 1; // Ensures page number is always at least 1
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 0 ? value : 10; // Ensures page size is always at least 10
        }
    }
}