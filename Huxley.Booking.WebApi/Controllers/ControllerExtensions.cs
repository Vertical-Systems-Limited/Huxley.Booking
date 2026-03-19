using FluentValidation;
using Huxley.Core;
using Microsoft.AspNetCore.Mvc;

namespace Huxley.Booking.WebApi.Controllers;

public static class ControllerExtensions
{
    
    public static ActionResult<TResult> ToActionResult<TResult>(this Result<TResult> result)
    {
        return result.AsEither<ActionResult<TResult>>(
            obj => new OkObjectResult(obj),
            ex =>
            {
                return ex switch
                {
                    ValidationException validationException => new BadRequestObjectResult(validationException.ToProblemDetails()),
                    _ => new BadRequestObjectResult(ex.ToProblemDetails())
                };
            }
        );
    }
}

internal static class ExceptionExtensions
{
    public static ValidationProblemDetails ToProblemDetails(this Exception exception)
    {
        var result = new ValidationProblemDetails();

        result.Errors.Add(new KeyValuePair<string, string[]>(
            exception.GetType().Name,
            new[] { exception.Message }
        ));

        return result;
    }   

    public static ValidationProblemDetails ToProblemDetails(this ValidationException validationException)
    {
        var result = new ValidationProblemDetails();

        foreach (var error in validationException.Errors)
        {

            if (result.Errors.ContainsKey(error.PropertyName))
            {
                result.Errors[error.PropertyName] =
                    result.Errors[error.PropertyName].Concat(new[] { error.ErrorMessage }).ToArray();
                continue;
            }

            result.Errors.Add(new KeyValuePair<string, string[]>(
                error.PropertyName,
                new[] { error.ErrorMessage }
            ));
        }

        return result;
    }
}