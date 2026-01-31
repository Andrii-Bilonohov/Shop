using Application.Abstractions.Repositories;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    public ReviewRepository(ItemContext context) : base(context) { }
    
    public async ValueTask<Review?> GetByItemAndUserAsync(Guid itemId, Guid userId, CancellationToken ct)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.ItemId == itemId && r.UserId == userId, ct);
    }
}