namespace Messenger.Application.Models.Pagination
{
    public class CursorPaginationParam
    {
        public string? Previous { get; set; }
        public string? Next { get; set; }
        public int PageSize { get; set; } = 20;
    }
}
