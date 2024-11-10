using FluentValidation;
using Messenger.Application.Models.Pagination;
using Messenger.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Messenger.Application.Extensions
{
    public static class QueryableExtensions
    {
        private static int maxPageSize = 100;
        private static string paginationKey = "X-Pagination";
        private static string cursorPaginationKey = "X-Cursor-Pagination";

        public static async Task<IQueryable<T>> ToPagedListAsync<T>(
            this IQueryable<T> source,
            HttpContext httpContext,
            int pageSize,
            int pageIndex)
        {
            if (pageSize <= 0 || pageIndex <= 0)
                throw new ValidationException("Page size or index should be greater than 0");

            if (pageSize > maxPageSize)
                throw new ValidationException($"Page size should be less than {maxPageSize}");

            var totalCount = await source.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalCount: totalCount,
                currentPage: pageIndex,
                pageSize: pageSize);

            httpContext.Response.Headers[paginationKey] = JsonSerializer
                .Serialize(paginationMetadata);

            return source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }

        public static async Task<List<T>> ToCursorPagedListAsync<T>(
            this IQueryable<T> source,
            HttpContext httpContext,
            int pageSize,
            string previousCursor = null,
            string nextCursor = null) where T : class, IAuditable
        {
            var totalCount = await source.CountAsync();

            DateTime decodedPreviousCursor = previousCursor != null
                ? CursorPaginationMetadata.DecodeCursor(previousCursor)
                : DateTime.MinValue;

            DateTime decodedNextCursor = nextCursor != null
                ? CursorPaginationMetadata.DecodeCursor(nextCursor) 
                : DateTime.MinValue;

            if (decodedPreviousCursor != DateTime.MinValue)
                source = source.Where(item => item.CreatedAt > decodedPreviousCursor);
                                                               // 10 dan yangilar
            else if (decodedNextCursor != DateTime.MinValue)
                source = source.Where(item => item.CreatedAt < decodedNextCursor);
            // 9 dan eskilar

            List<T> items = source
                .OrderByDescending(item => item.CreatedAt)
                .Take(pageSize + 1)
                .ToList(); // 10, 9 , 8                       // 8 7 6

            bool hasMoreItems = items.Count > pageSize;

            if (hasMoreItems)
                items = items.Take(pageSize).ToList(); // 10 , 9 // 8 7

            // 10
            var firstCursor = items.FirstOrDefault() != null ? items.First().CreatedAt : DateTime.MinValue;
            // 9
            var lastCursor = items.LastOrDefault() != null ? items.Last().CreatedAt : DateTime.MinValue;

            var metadata = new CursorPaginationMetadata(
                previous: firstCursor != DateTime.MinValue ? CursorPaginationMetadata.EncodeCursor(firstCursor) : null,
                next: lastCursor != DateTime.MinValue && hasMoreItems ? CursorPaginationMetadata.EncodeCursor(lastCursor) : null,
                pageSize: pageSize,
                hasMorePages: hasMoreItems,
                totalCount: totalCount
                );

            httpContext.Response.Headers[cursorPaginationKey] = JsonSerializer
                .Serialize(metadata);

            return items;
        }
    }
}
