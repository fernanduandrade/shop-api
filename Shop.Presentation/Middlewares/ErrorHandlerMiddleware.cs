using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Shop.Presentation.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);    
        }
        catch(ValidationException ex)
        {
            var validationErrors = GetBadRequestValidation(ex);

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            await response.WriteAsync(JsonConvert.SerializeObject(validationErrors));
        }
        catch (Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            await response.WriteAsync(JsonConvert.SerializeObject(response));
        }
        
    }
    private ValidationProblemDetails GetBadRequestValidation(ValidationException ex)
    {
        var traceId = Guid.NewGuid().ToString();

        var errors = new Dictionary<string, string[]>();

        foreach(var error in ex.Errors)
        {
            errors.Add(error.PropertyName, new string[] { error.ErrorMessage });
        }

        var validationErrors = new ValidationProblemDetails(errors);

        validationErrors.Status = (int)HttpStatusCode.BadRequest;
        validationErrors.Type = "https://httpstatuses.com/400";
        validationErrors.Title = "Validação fahou";
        validationErrors.Detail = "Um ou mais errors ocorreram por favor e precisam ser corrigidos. Por favor olhe os detalhes";
        validationErrors.Instance = traceId;


        return validationErrors;

    }
}