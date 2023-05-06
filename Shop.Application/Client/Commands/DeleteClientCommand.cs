using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Common.Interfaces;
using Shop.Application.Common.Models;

namespace Shop.Application.Client.Commands;

public sealed record DeleteClientCommand :IRequest<ApiResult>
{
    public int Id { get; init; }
}

public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, ApiResult>
{
    private readonly IAppDbContext _context;

    public DeleteClientCommandHandler(IAppDbContext context)
        => (_context) = (context);
    public async Task<ApiResult> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Clients.FirstOrDefaultAsync(
            quiz => quiz.Id == request.Id);

        if (entity is null) return new ApiResult("Error while trying to delete the register.", ResponseTypeEnum.Error);

        _context.Clients.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return new ApiResult("Operation completed successfully.", ResponseTypeEnum.Success);
    }
}