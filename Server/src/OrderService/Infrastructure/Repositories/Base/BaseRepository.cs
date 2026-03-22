using System.Linq.Expressions;
using Application.Abstractions.Repositories.Base;
using Application.DTOs.Base;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        protected readonly OrderContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(OrderContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dbSet.Update(entity);
            entity.Touch();
        }

        public void Delete(Guid id)
        {
            var entity = _dbSet.Find(id);

            if (entity is null)
                throw new KeyNotFoundException($"Entity with id {id} not found.");

            _dbSet.Remove(entity);
        }

        public async ValueTask<T?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id, ct);
        }

        public async ValueTask<T?> GetByAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken ct)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<BaseResponse<T>> GetAllAsync(int limit, int offset, CancellationToken ct)
        {
            var query = _dbSet.AsNoTracking();

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .Skip(offset)
                .Take(limit)
                .ToListAsync(ct);

            return new BaseResponse<T>(items, totalCount);
        }
    }
}
