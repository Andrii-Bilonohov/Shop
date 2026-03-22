using Application.Filters;
using Application.Contracts.Base;
using Application.DTOs.Items.Requests;
using Application.DTOs.Items.Responses;
namespace Application.Abstractions.Services
{
    public interface IItemService
    {
        Task<ResponseList<ItemResponse>> GetAllAsync(int limit, int offset, ItemFilter? filter, CancellationToken ct);
        ValueTask<Response<ItemResponse>> GetByIdAsync(Guid id, CancellationToken ct);
        Task<Information> CreateAsync(CreateItem item, CancellationToken ct);
        Task<Information> UpdateAsync(Guid id, UpdateItem item, CancellationToken ct);
        Task<Information> DeleteAsync(Guid id, CancellationToken ct);
    }
}
