using MediatR;
using Shop.Application.Common.Interfaces;
using Shop.Application.Common.Models;
using Shop.Domain.SalesHistory;

namespace Shop.Application.SalesHistory.DeleteSaleHistory;

public sealed class DeleteSaleHistoryCommandHandler : IRequestHandler<DeleteSaleHistoryCommand, ApiResult>
{
    private readonly ISaleHistoryRepository _saleHistoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSaleHistoryCommandHandler(IUnitOfWork unitOfWork, ISaleHistoryRepository saleHistoryRepository)
        => (_unitOfWork, _saleHistoryRepository) = (unitOfWork, saleHistoryRepository);
    public async Task<ApiResult> Handle(DeleteSaleHistoryCommand request, CancellationToken cancellationToken)
    {
        await _saleHistoryRepository.Remove(request.Id);
        await _unitOfWork.Commit(cancellationToken);

        return new ApiResult(true, ResponseTypeEnum.Success, "Error while trying to delete the register.");
    }
}