using AutoMapper;
using MediatR;
using Shop.Application.Common.Models;
using Shop.Application.Products.Dtos;
using Shop.Domain.Products;

namespace Shop.Application.Products.GetProductById;

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ApiResult<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
        => (_productRepository, _mapper) = (productRepository, mapper);

    public async Task<ApiResult<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _productRepository.FindByIdAsync(request.Id);
        if (result is null)
            return new ApiResult<ProductDto>(null, ResponseTypeEnum.Warning, "Não encontrado.");

        var dto = _mapper.Map<ProductDto>(result);

        return new ApiResult<ProductDto>(dto, ResponseTypeEnum.Success, "Sucesso.");
    }
}