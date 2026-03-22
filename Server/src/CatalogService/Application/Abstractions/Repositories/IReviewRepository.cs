using Application.Abstractions.Repositories.Base;
using Application.DTOs.Reviews.Requests;
using Domain.Models;

namespace Application.Abstractions.Repositories;

public interface IReviewRepository : IBaseRepository<Review>
{
    ValueTask<Review?> GetByItemAndUserAsync(Guid itemId, Guid userId, CancellationToken ct);
}