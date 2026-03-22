

using Application.DTOs.Reviews.Requests;
using Application.DTOs.Reviews.Responses;
using Domain.Models;

namespace Application.Mappers;

public static class ReviewMapper
{
    public static ReviewResponse ToResponse(this Review review)
    {
        return new ReviewResponse
        (
            review.ItemId,
            review.UserId,
            review.Rating,
            review.Description,
            review.ImageUrl
        );
    }

    public static List<ReviewResponse> ToResponse(this ICollection<Review> reviews)
    {
        return reviews.Select(ToResponse).ToList();
    }

    public static Review ToReview(this ReviewItemRequest request)
    {
        return new Review
        (
            request.ItemId, 
            request.UserId, 
            request.Rating, 
            request.Description, 
            request.ImageUrl
        );
    }
}