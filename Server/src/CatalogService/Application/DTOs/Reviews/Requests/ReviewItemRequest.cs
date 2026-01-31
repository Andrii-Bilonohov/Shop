namespace Application.DTOs.Reviews.Requests;

public record ReviewItemRequest(Guid ItemId, Guid UserId, int Rating, string? Description, string? ImageUrl );