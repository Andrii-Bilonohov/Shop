using Application.Abstractions.Repositories.Base;
using Application.Filters;
using Domain.Models;

namespace Application.Abstractions.Repositories
{
    public interface IItemRepository : IBaseRepository<Item> 
    {
        Task<(IReadOnlyList<Item> Items, int TotalCount)> GetAllAsync(int limit, int offset, ItemFilter? filter, CancellationToken ct);
    }
}
