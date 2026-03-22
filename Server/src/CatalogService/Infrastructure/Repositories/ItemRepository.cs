using Application.Abstractions.Repositories;
using Application.DTOs.Base.Response;
using Application.Filters;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public ItemRepository(ItemContext context)
            : base(context){}

        public async Task<BaseResponse<Item>> GetAllAsync(int limit, int offset, ItemFilter? filter, CancellationToken ct)
        {
            IQueryable<Item> query = _dbSet.AsNoTracking();

            query = ApplyFilter(query, filter);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderBy(i => i.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(ct);

            return new BaseResponse<Item>(items, totalCount);
        }

        private static IQueryable<Item> ApplyFilter(
            IQueryable<Item> query,
            ItemFilter? filter)
        {
            if (filter is null)
                return query;

            if (filter?.Category is not null)
            {
                query = query.Where(i => i.Category == filter.Category);
            }

            return query;
        }
    }
}
