using Application.DTOs.Reviews.Requests;
using Application.DTOs.Reviews.Responses;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles;

public class ReviewProfile  : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewResponse>()
            .ForCtorParam(nameof(ReviewResponse.ItemId), opt => opt.MapFrom(s => s.ItemId))
            .ForCtorParam(nameof(ReviewResponse.UserId), opt => opt.MapFrom(s => s.UserId))
            .ForCtorParam(nameof(ReviewResponse.Rating), opt => opt.MapFrom(s => s.Rating))
            .ForCtorParam(nameof(ReviewResponse.Description), opt => opt.MapFrom(s => s.Description))
            .ForCtorParam(nameof(ReviewResponse.ImageUrl), opt => opt.MapFrom(s => s.ImageUrl));

        CreateMap<ReviewItemRequest, Review>()
            .ForMember(d => d.ItemId, opt => opt.Ignore())
            .ForMember(d => d.UserId, opt => opt.Ignore())
            .ForMember(d => d.Item, opt => opt.Ignore())
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.Ignore())
            .ForMember(d => d.UpdatedAt, opt => opt.Ignore())
            .ForMember(d => d.IsDeleted, opt => opt.Ignore());
    }
}