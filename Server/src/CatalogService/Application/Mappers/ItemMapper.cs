using Application.DTOs.Items.Requests;
using Application.DTOs.Items.Responses;
using Domain.Models;

namespace Application.Mappers
{
    public static class ItemMapper
    {
        public static ItemResponse ToResponse(this Item item)
        {
            return new ItemResponse(
                Id: item.Id,
                Name: item.Name,
                Description: item.Description,
                Category: item.Category,
                Price: item.Price,
                Stock: item.Stock,
                Weight: item.Weight,
                ImageUrl: item.ImageUrl,
                Reviews: item.Reviews
            );
        }
        
        public static IReadOnlyList<ItemResponse> ToResponse(this IEnumerable<Item> items)
        {
            return items.Select(item => item.ToResponse()).ToList();
        }

        public static Item ToItem(this CreateItem create)
        {
            return new Item(
                create.Name,
                create.Description,
                create.Category,
                create.Price,
                create.Stock,
                create.Weight,
                create.ImageUrl
            );
        }
    }
}
