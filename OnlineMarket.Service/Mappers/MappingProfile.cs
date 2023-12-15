using AutoMapper;
using OnlineMarket.Domain.Entities.Carts;
using OnlineMarket.Domain.Entities.Categories;
using OnlineMarket.Domain.Entities.Filials;
using OnlineMarket.Domain.Entities.Orders;
using OnlineMarket.Domain.Entities.Products;
using OnlineMarket.Domain.Entities.Users;
using OnlineMarket.Service.DTOs.Carts;
using OnlineMarket.Service.DTOs.Categories;
using OnlineMarket.Service.DTOs.Filials;
using OnlineMarket.Service.DTOs.Orders;
using OnlineMarket.Service.DTOs.Products;
using OnlineMarket.Service.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMarket.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //User
        CreateMap<User, UserCreationDto>().ReverseMap();
        CreateMap<User, UserUpdateDto>().ReverseMap();
        CreateMap<User, UserResultDto>().ReverseMap();
        
        //Product
        CreateMap<Product, ProductCreationDto>().ReverseMap();
        CreateMap<Product, ProductUpdateDto>().ReverseMap();
        CreateMap<Product, ProductResultDto>().ReverseMap();
        
        //Order
        CreateMap<Order, OrderCreationDto>().ReverseMap();
        CreateMap<Order, OrderUpdateDto>().ReverseMap();
        CreateMap<Order, OrderResultDto>().ReverseMap();

        //OrderItem
        CreateMap<OrderItem, OrderItemCreationDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemUpdateDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemResultDto>().ReverseMap();
        
        //Category
        CreateMap<Category, CategoryCreationDto>().ReverseMap();
        CreateMap<Category, CategoryUpdateDto>().ReverseMap();
        CreateMap<Category, CategoryResultDto>().ReverseMap();
        
        //CartItem
        CreateMap<CartItem, CartItemCreationDto>().ReverseMap();
        CreateMap<CartItem, CartItemUpdateDto>().ReverseMap();
        CreateMap<CartItem, CartItemResultDto>().ReverseMap();
        
        //Cart
        CreateMap<Cart, CartResultDto>().ReverseMap();

        //Filial
        CreateMap<Filial, FilialCreationDto>().ReverseMap();
        CreateMap<Filial, FilialCustomDto>().ReverseMap();
    }
}
