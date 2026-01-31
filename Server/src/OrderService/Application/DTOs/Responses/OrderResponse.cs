using Domain.Enums;

namespace Application.DTOs.Responses
{
    public record OrderResponse
    (
        Guid Id,
        Guid UserId,
        string Title,
        string Description,
        decimal TotalPrice,
        int TotalItems,
        OrderStatus Status
    );
}
