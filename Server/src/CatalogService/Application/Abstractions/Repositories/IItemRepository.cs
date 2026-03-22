using Application.Abstractions.Repositories.Base;
using Application.DTOs.Base.Response;
using Application.Filters;
using Domain.Models;

namespace Application.Abstractions.Repositories
{
    public interface IItemRepository : IBaseRepository<Item> 
    {
        Task<BaseResponse<Item>> GetAllAsync(int limit, int offset, ItemFilter? filter, CancellationToken ct);
    }
}
