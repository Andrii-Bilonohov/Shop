using Domain.Enums;

namespace Application.DTOs.Items.Responses
{
    public record ItemResponse
    (
    Guid Id,
    string Name,
    string Description,
    Category Category,
    int? Rating,
    decimal Price,
    int Stock,
    double Weight,
    string ImageUrl
    );

}
