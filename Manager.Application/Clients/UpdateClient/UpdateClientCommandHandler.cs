using AutoMapper;
using MediatR;
using Manager.Application.Clients.Dtos;
using Manager.Application.Common.Interfaces;
using Manager.Application.Common.Models;
using Manager.Domain.Clients;

namespace Manager.Application.Clients.UpdateClient;

public sealed class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, ApiResult<ClientDto>>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateClientCommandHandler(IClientRepository clientRepository, IMapper mapper, IUnitOfWork unitOfWork)
        =>(_clientRepository, _mapper, _unitOfWork) = (clientRepository, mapper, unitOfWork);

    public async Task<ApiResult<ClientDto>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.FindByIdAsync(request.Id);
        client.Update(request.Name, request.LastName, request.Phone, request.IsActive, request.Credit, request.Debt);
        _clientRepository.Update(client);
        await _unitOfWork.Commit(cancellationToken);

        var dto = _mapper.Map<ClientDto>(client);
        
        return new ApiResult<ClientDto>(dto, ResponseTypeEnum.Success ,"Concluido com sucesso");
    }
}