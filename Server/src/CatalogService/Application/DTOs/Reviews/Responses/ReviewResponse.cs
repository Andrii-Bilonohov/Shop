namespace Application.DTOs.Reviews.Responses;

public record ReviewResponse(Guid ItemId, Guid UserId, int Rating, string? Description, string? ImageUrl );