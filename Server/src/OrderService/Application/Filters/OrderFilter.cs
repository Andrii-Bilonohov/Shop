using Domain.Enums;

namespace Application.Filters
{
    public record OrderFilter
    (
        OrderStatus? Status
    );
}
