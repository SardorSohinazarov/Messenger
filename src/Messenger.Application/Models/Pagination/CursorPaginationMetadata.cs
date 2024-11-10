using System.Text;

namespace Messenger.Application.Models.Pagination
{
    public class CursorPaginationMetadata
    {
        public CursorPaginationMetadata(
            int pageSize,
            string previous,
            string next,
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
        public string Previous { get; }
        public string Next { get; }

        public static string EncodeCursor(DateTime value) => 
            value != DateTime.MinValue
                ? Convert.ToBase64String(BitConverter.GetBytes(value.Ticks))
                : null;

        public static DateTime DecodeCursor(string cursor) =>
            new DateTime(BitConverter.ToInt64(Convert.FromBase64String(cursor), 0), DateTimeKind.Utc);
    }
}
