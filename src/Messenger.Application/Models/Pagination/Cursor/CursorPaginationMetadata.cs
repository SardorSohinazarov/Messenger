namespace Messenger.Application.Models.Pagination.Cursor
{
    public class CursorPaginationMetadata
    {
        public CursorPaginationMetadata(
            int pageSize,
            DateTime previous,
            DateTime next,
            bool hasMorePages,
            int totalCount)
        {
            PageSize = pageSize;
            Previous = previous;
            Next = next;
            HasMorePages = hasMorePages;
            TotalCount = totalCount;
        }

        public int TotalCount { get; set; }
        public bool HasMorePages { get; set; }
        public int PageSize { get; set; }
        public DateTime Previous { get; }
        public DateTime Next { get; }
    }
}
