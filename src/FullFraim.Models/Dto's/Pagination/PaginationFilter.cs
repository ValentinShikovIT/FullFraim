namespace FullFraim.Models.Dto_s.Pagination
{
    public class PaginationFilter
    {
        private const int MaxPageSize = 50;
        private const int MinPageNumber = 1;
        private const int DefaultPageNumber = 1;
        private const int DefaultPageSize = 10;

        private int pageNumber;
        private int pageSize;

        public PaginationFilter()
        {
            this.pageNumber = DefaultPageNumber;
            this.pageSize = DefaultPageSize;
        }

        public PaginationFilter(int pageNumber, int pageSize)
            : this()
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public int PageNumber
        {
            get => pageNumber;
            set => pageNumber = value < MinPageNumber ? MinPageNumber : value;
        }

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
