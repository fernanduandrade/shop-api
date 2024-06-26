using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Manager.Application.Common.Models;

namespace Manager.Presentation.Tools;

public class ValidationModelStateAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var modelStateEntries = context.ModelState.Values;

            foreach (var entry in modelStateEntries)
            {
                foreach (var error in entry.Errors)
                {
                    ApiResult<string> resultObject = new(error.ErrorMessage, ResponseTypeEnum.Warning);
                    context.Result = new JsonResult(resultObject);
                }
            }
        }
    }
}