using AutoMapper;
using Manager.Application.Common.Mapping;
using Manager.Application.Products.Dtos;
using Manager.Domain.Orders;

namespace Manager.Application.Orders.Dtos;

public sealed record OrderDto : IMapFrom<Order>
{
    public Guid Id { get; init; }
    public string? ClientName { get; init; }
    public OrderStatus Status { get; init; }
    public List<ProductDto> Products { get; init; }
    public DateTime Date {get; init;}

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Order, OrderDto>()
            .ForMember(
                dest => dest.Products,
                opt =>
                    opt.MapFrom(src => src.OrderProducts.Select(op => new ProductDto
                    {
                        Id = op.Product.Id,
                        Price = op.Product.Price, 
                        Name = op.Product.Name,
                        Quantity = op.Quantity
                    }).ToList())
            )
            .ForMember(x => x.Date,
            opt =>
                opt.MapFrom(src => src.Created));
    }
}