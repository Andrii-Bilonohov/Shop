using Application.Abstractions.Repositories.Base;
using Application.DTOs.Base;
using Application.Filters;
using Domain.Models;

namespace Application.Abstractions.Repositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<BaseResponse<Order>> GetAllAsync(int limit, int offset, OrderFilter filter, CancellationToken ct);
    }
}
