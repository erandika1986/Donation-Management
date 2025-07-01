using Microsoft.EntityFrameworkCore;
using ViharaFund.Application.Constants;

namespace ViharaFund.Application.DTOs.Common
{
    public class PaginatedListDto<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public PaginatedListDto(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public static async Task<PaginatedListDto<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - NumberConstant.One) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedListDto<T>(items, count, pageNumber, pageSize);
        }

        public static PaginatedListDto<T> ToPagedList
        (
            IEnumerable<T> source,
            int pageNumber,
            int pageSize
        )
        {
            var count = source.Count();
            var items = source
              .Skip((pageNumber - NumberConstant.One) * pageSize)
              .Take(pageSize).ToList();
            return new PaginatedListDto<T>(items, count, pageNumber, pageSize);
        }

        public static PaginatedListDto<T> ToClientArrayPagedList
        (
            IEnumerable<T> source,
            int pageNumber,
            int pageSize
        )
        {
            var count = source.Count();
            var items = source
              .Skip((pageNumber + NumberConstant.One) * pageSize)
              .Take(pageSize).ToList();
            return new PaginatedListDto<T>(items, count, pageNumber, pageSize);
        }
    }
}
