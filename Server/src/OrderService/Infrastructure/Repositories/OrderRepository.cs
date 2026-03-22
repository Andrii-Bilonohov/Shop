using Application.Abstractions.Repositories;
using Application.DTOs.Base;
using Application.Filters;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext context)
            : base(context) { }

        public async Task<BaseResponse<Order>> GetAllAsync(
             int limit,
             int offset,
             OrderFilter? filter,
             CancellationToken ct)
        {
            IQueryable<Order> query = _dbSet.AsNoTracking();

            query = ApplyFilter(query, filter);

            var totalCount = await query.CountAsync(ct);

            var orders = await query
                .OrderBy(o => o.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(ct);

            return new  BaseResponse<Order>(orders, totalCount);
        }

        private static IQueryable<Order> ApplyFilter(
            IQueryable<Order> query,
            OrderFilter? filter)
        {
            if (filter is null)
                return query;

            if (filter.Status is not null)
                query = query.Where(o => o.Status == filter.Status);
            
            return query;
        }
    }
}
