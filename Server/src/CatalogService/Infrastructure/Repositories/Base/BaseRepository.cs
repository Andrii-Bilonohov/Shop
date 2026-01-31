using System.Linq.Expressions;
using Application.Abstractions.Repositories.Base;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Base
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        protected readonly ItemContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ItemContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _dbSet.Add(entity);
        }

        public void Delete(Guid id)
        {
            var entity = _dbSet.Find(id);

            if (entity is null)
            {
                throw new KeyNotFoundException($"Entity with id {id} not found.");
            }

            _dbSet.Remove(entity);
        }

        public async Task<(IReadOnlyList<T> Entities, int TotalCount)> GetAllAsync(int limit, int offset, CancellationToken ct)
        {
            IQueryable<T> query = _dbSet;

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .AsNoTracking()
                .OrderBy(i => i.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        public async ValueTask<T?> GetByIdAsync(Guid id, CancellationToken ct)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct);
        }

        public void Update(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
                entry.State = EntityState.Modified;
            }

            entity.Touch();
        }

        public async ValueTask<T?> GetByAsync(Expression<Func<T, bool>> predicate, CancellationToken ct)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, ct);
        }
    }
}
