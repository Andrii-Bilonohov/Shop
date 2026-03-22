namespace Application.DTOs.Requests
{
    public record CreateOrder
    (
        Guid UserId,
        string Title,
        string Description,
        decimal TotalPrice,
        int TotalItems,
        List<Guid> ItemsId
    );
}
