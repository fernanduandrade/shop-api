using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Manager.Presentation.Tools;

namespace Manager.Presentation.Controllers.Base;

[ValidationModelState]
[ApiController]
[Route("api/v{version:ApiVersion}/[controller]")]
public abstract class BaseController : ControllerBase
{
    private ISender _mediator = null;

    protected ISender Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}   