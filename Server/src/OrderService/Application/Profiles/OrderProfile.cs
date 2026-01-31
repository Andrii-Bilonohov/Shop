using Application.DTOs.Requests;
using Application.DTOs.Responses;
using AutoMapper;
using Domain.Models;

namespace Application.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile() 
        {
            CreateMap<Order, OrderResponse>();

            CreateMap<CreateOrder, Order>()
                .ReverseMap();
            CreateMap<UpdateOrderStatus, Order>()
                .ReverseMap();
        }
    }
}
