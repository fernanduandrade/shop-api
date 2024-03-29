using AutoMapper;
using MediatR;
using Shop.Application.Common.Models;
using Shop.Application.Orders.Dtos;
using Shop.Domain.Orders;

namespace Shop.Application.Orders.GetOrderPaginated;

public class GetOrderStatusPaginatedQueryHandler
    : IRequestHandler<GetOrderStatusPaginatedQuery, ApiResult<PaginatedList<OrderDto>>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderStatusPaginatedQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    public async Task<ApiResult<PaginatedList<OrderDto>>> Handle(GetOrderStatusPaginatedQuery request,
        CancellationToken cancellationToken)
    {
        var records = _orderRepository.GetAllByStatus(request.Status);
        var result = await PaginatedList<OrderDto>
            .CreateAsync(records, request.PageNumber, request.PageSize, _mapper);
        return new ApiResult<PaginatedList<OrderDto>>(result, ResponseTypeEnum.Success, "Sucesso");
    }
}