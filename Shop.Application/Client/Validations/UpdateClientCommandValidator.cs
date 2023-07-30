using FluentValidation;
using Shop.Application.Client.Commands;
using Shop.Application.Product.Validations;

namespace Shop.Application.Client.Validations;

public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Phone).NotEmpty();
        RuleFor(x => x.IsActive).NotNull();
    }
}