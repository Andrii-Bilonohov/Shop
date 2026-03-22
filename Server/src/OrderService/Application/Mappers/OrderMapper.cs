using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Domain.Models;

namespace Application.Mappers
{
    public static class OrderMapper
    {
        public static OrderResponse ToResponse(this Order order)
        {
            return new OrderResponse
            (
                order.Id,
                order.UserId,
                order.Title,
                order.Description,
                order.TotalPrice,
                order.TotalItems,
                order.Status,
                order.ItemsId,
                order.CreatedAt,
                order.UpdatedAt
            );
        }

        public static List<OrderResponse> ToResponse(this ICollection<Order> orders)
        {
            return orders.Select(ToResponse).ToList();
        }

        public static Order ToOrder(this CreateOrder create)
        {
            return new Order
            (
                create.UserId,
                create.Title,
                create.Description,
                create.TotalPrice,
                create.TotalItems,
                create.ItemsId
            );
        }
    }
}
