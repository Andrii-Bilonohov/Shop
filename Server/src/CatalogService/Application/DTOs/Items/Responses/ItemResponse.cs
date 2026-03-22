using Domain.Enums;
using Domain.Models;

namespace Application.DTOs.Items.Responses
{
    public record ItemResponse
    (
        Guid Id,
        string Name,
        string Description,
        Category Category,
        decimal Price,
        int Stock,
        double Weight,
        string ImageUrl,
        ICollection<Review?> Reviews
    );

}
