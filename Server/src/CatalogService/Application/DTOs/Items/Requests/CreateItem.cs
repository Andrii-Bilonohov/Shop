using Domain.Enums;

namespace Application.DTOs.Items.Requests
{
    public record CreateItem
    (
        string Name,
        string Description,
        Category Category,
        decimal Price,
        int Stock,
        double Weight,
        string ImageUrl
     );
}
