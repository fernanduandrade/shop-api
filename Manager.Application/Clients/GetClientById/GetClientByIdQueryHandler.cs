using AutoMapper;
using MediatR;
using Manager.Application.Clients.Dtos;
using Manager.Application.Common.Models;
using Manager.Domain.Clients;

namespace Manager.Application.Clients.GetClientById;

public sealed class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery ,ApiResult<ClientDto>>
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public GetClientByIdQueryHandler(IClientRepository clientRepository, IMapper mapper)
        => (_clientRepository, _mapper) = (clientRepository, mapper);
    public async Task<ApiResult<ClientDto>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _clientRepository.FindByIdAsync(request.Id);

        if(result is null)
            return new ApiResult<ClientDto>(null, ResponseTypeEnum.Warning,"Failed to find the record.");

        var dto = _mapper.Map<ClientDto>(result);
        
        return new ApiResult<ClientDto>(dto, ResponseTypeEnum.Success,"Operation completed successfully.");
    }
}