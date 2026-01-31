using Application.Abstractions.Repositories.Base;
using Application.Filters;
using Domain.Models;

namespace Application.Abstractions.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<(IReadOnlyList<Order> Orders, int TotalCount)> GetAllAsync(int limit, int offset, OrderFilter filter, CancellationToken ct);
    }
}
