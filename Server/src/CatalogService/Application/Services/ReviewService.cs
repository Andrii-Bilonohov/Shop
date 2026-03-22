using Application.Abstractions.Logger;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Abstractions.UnitOfWork;
using Application.Contracts.Base;
using Application.DTOs.Base.Response;
using Application.DTOs.Reviews.Requests;
using Application.DTOs.Reviews.Responses;
using Application.Mappers;
using Domain.Models;

namespace Application.Services;

public sealed class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExceptionLogger _logger;
    
    private const int MaxLimit = 100;
    private const int MinOffset = 0;

    public ReviewService(
        IReviewRepository reviewRepository,
        IUnitOfWork unitOfWork,
        IItemRepository itemRepository,
        IExceptionLogger logger)
    {
        _reviewRepository = reviewRepository ?? throw new ArgumentNullException(nameof(reviewRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<Information> RateItemAsync(ReviewItemRequest request, CancellationToken ct = default)
    {
        try
        {
            var existing = await _reviewRepository
                .GetByItemAndUserAsync(request.ItemId, request.UserId, ct);

            if (existing is not null)
                return new Information(Error: "You already rated this item");

            var review = request.ToReview();

            _reviewRepository.Add(review);
            await _unitOfWork.SaveChangesAsync(ct);

            return new Information(Id: review.Id, Message: "Item rated");
        }
        catch (OperationCanceledException)
        {
            return new Information(Error: "Request canceled");
        }
        catch (Exception ex)
        {
            _logger.Log(ex, "failed rate item", request.ItemId);
            return new Information(Error: "Unexpected error: " + ex.Message);
        }
    }
    
    public async Task<Information> UpdateRateItemAsync(ReviewItemRequest request, CancellationToken ct = default)
    {
        try
        {
            var review = await _reviewRepository
                .GetByItemAndUserAsync(request.ItemId, request.UserId, ct);

            if (review is null)
                return new Information(Error: "Review not found");
            
            review.Update(request.Rating, request.Description, request.ImageUrl);

            _reviewRepository.Update(review);
            await _unitOfWork.SaveChangesAsync(ct);

            return new Information(Id: review.Id, Message: "Review updated");
        }
        catch (Exception ex)
        {
            _logger.Log(ex, "failed update review", request.ItemId);
            return new Information(Error: "Unexpected error: " + ex.Message);
        }
    }
    
    public async Task<Information> DeleteItemAsync(Guid id, CancellationToken ct = default)
    {
        try
        {
            var review = await _reviewRepository.GetByIdAsync(id, ct);

            if (review is null)
                return new Information(Error: "Review not found");

            _reviewRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync(ct);

            return new Information(Message: "Review deleted");
        }
        catch (Exception ex)
        {
            _logger.Log(ex, "failed delete review", id);
            return new Information(Error: "Unexpected error: " + ex.Message);
        }
    }
    
    public async Task<ResponseList<ReviewResponse>> GetReviewsAsync(int limit, int offset, CancellationToken ct = default)
    {
        try
        {
            
            limit = Math.Clamp(limit, 1, MaxLimit);
            offset = Math.Max(MinOffset, offset);
            
            var response = await _reviewRepository
                .GetAllAsync(limit, offset, ct);

            var data = response.Items.ToResponse();
            
            var totalPages = response.TotalCount == 0 ? 0 : (int)Math.Ceiling(response.TotalCount / (double)limit);
            
            var result = new ResponseList<ReviewResponse>(
                Limit: limit,
                Offset: offset,
                Items: response.TotalCount,
                Pages: totalPages,
                Data: data
            );
            
            return result;
        }
        catch (OperationCanceledException)
        {
            return new ResponseList<ReviewResponse>(Error: "Request canceled");
        }
        catch (ArgumentNullException ex)
        {
            return new ResponseList<ReviewResponse>(Error: ex.Message);
        }
        catch (ArgumentException ex)
        {
            return new ResponseList<ReviewResponse>(Error: ex.Message);
        }
        catch (Exception ex)
        {
            _logger.Log(ex, "GetAll items failed");
            return new ResponseList<ReviewResponse>(Error: "Unexpected error: " + ex.Message);
        }
    }
}