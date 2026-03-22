using Application.Contracts.Base;
using Application.DTOs.Reviews.Requests;
using Application.DTOs.Reviews.Responses;

namespace Application.Abstractions.Services;

public interface IReviewService
{
    Task<Information> RateItemAsync(ReviewItemRequest request, CancellationToken ct = default);
    Task<Information> UpdateRateItemAsync(ReviewItemRequest request,  CancellationToken ct = default);
    Task<Information> DeleteItemAsync(Guid id, CancellationToken ct = default);
    Task<ResponseList<ReviewResponse>> GetReviewsAsync(int limit, int offset, CancellationToken ct = default);
}