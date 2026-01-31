using Application.DTOs.Items.Requests;
using Application.DTOs.Items.Responses;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile() 
        {
            CreateMap<CreateItem, Item>();
            CreateMap<UpdateItem, Item>();

            CreateMap<Item, ItemResponse>()
                .ReverseMap();
        }
    }
}
