using Application.Contracts.Base;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Filters;

namespace Application.Abstractions.Services
{
    public interface IOrderService
    {
        Task<ResponseList<OrderResponse>> GetAllAsync(int limit, int offset, OrderFilter filter, CancellationToken ct);
        ValueTask<Response<OrderResponse>> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Information> CreateAsync(CreateOrder order, CancellationToken ct);
        Task<Information> UpdateOrderStatusAsync(Guid id, UpdateOrderStatus orderStatus, CancellationToken ct);
        Task<Information> DeleteAsync(Guid id, CancellationToken ct);
    }
}
