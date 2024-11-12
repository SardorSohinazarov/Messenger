namespace Messenger.Application.Models.Pagination.Cursor
{
    public class CursorPaginationParam
    {
        public DateTime Previous { get; set; }
        public DateTime Next { get; set; }
        public int PageSize { get; set; } = 20;
    }
}
