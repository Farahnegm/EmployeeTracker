namespace CodeZone.BLL.DTOs
{
    public class PaginationDto<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; } = 0;
        public int TotalPages { get; set; } = 0;
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int StartPage => Math.Max(1, CurrentPage - 2);
        public int EndPage => Math.Min(TotalPages, CurrentPage + 2);
    }
} 