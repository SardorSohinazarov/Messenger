using FluentValidation;
using Messenger.Application.Models.Pagination;
using Messenger.Application.Models.Pagination.Cursor;
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
            DateTime previousCursor,
            DateTime nextCursor) where T : class, IAuditable
        {
            if (pageSize > maxPageSize)
                throw new ValidationException($"Page size should be less than {maxPageSize}");

            var totalCount = await source.CountAsync();

            if (previousCursor != default)
                source = source.Where(item => item.CreatedAt > previousCursor);
            else if (nextCursor != default)
                source = source.Where(item => item.CreatedAt < nextCursor);

            List<T> items = null;
            if(previousCursor != default)
            {
                items = source
                    .OrderBy(item => item.CreatedAt)
                    .Take(pageSize + 1)
                    .ToList();
            }
            else
            {
                items = source
                    .OrderByDescending(item => item.CreatedAt)
                    .Take(pageSize + 1)
                    .ToList();
            }

            bool hasMoreItems = items.Count > pageSize;

            if (hasMoreItems)
                items = items.Take(pageSize).ToList();

            var firstCursor = items.FirstOrDefault() != null ? items.First().CreatedAt : default;
            var lastCursor = items.LastOrDefault() != null ? items.Last().CreatedAt : default;

            var metadata = new CursorPaginationMetadata(
                previous: firstCursor,
                next: lastCursor,
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
