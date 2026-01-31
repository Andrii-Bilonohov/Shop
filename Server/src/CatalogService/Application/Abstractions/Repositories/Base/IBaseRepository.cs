using System.Linq.Expressions;
using Domain.Models;

namespace Application.Abstractions.Repositories.Base
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        ValueTask<T?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<(IReadOnlyList<T> Entities, int TotalCount)> GetAllAsync(int limit, int offset, CancellationToken ct);
        ValueTask<T?> GetByAsync(Expression<Func<T, bool>> predicate, CancellationToken ct);
        void Add(T entity);
        void Update(T entity);
        void Delete(Guid id);
    }
}
