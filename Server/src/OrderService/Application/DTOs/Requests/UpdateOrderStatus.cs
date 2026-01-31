namespace Application.DTOs.Requests
{
    public enum OrderStatusAction
    {
        Pay,
        Ship,
        Complete,
        Cancel
    }

    public record UpdateOrderStatus
    (
        OrderStatusAction Action
    );
}
