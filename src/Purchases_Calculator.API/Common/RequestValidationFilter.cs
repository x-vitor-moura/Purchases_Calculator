using FluentValidation;

namespace Purchases_Calculator.API.Common;

public class RequestValidationFilter<TRequest>(ILogger<RequestValidationFilter<TRequest>> logger, IValidator<TRequest>? validator = null) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var requestName = typeof(TRequest).FullName;

        if (validator is null)
        {
            logger.LogInformation("{Request}: No validator configured.", requestName);
            return await next(context);
        }

        logger.LogInformation("{Request}: Validating...", requestName);
        var request = context.Arguments.OfType<TRequest>().First();

        var validationResult = await validator.ValidateAsync(request, context.HttpContext.RequestAborted);

        if (!validationResult.IsValid)
        {
            logger.LogWarning("{Request}: Validation failed.", requestName);
            var errors = new Dictionary<string, string[]>();
            foreach (var error in validationResult.Errors)
            {
                if (!errors.ContainsKey(error.PropertyName))
                {
                    errors[error.PropertyName] = new[] { error.ErrorMessage };
                }
                else
                {
                    errors[error.PropertyName] = errors[error.PropertyName].Append(error.ErrorMessage).ToArray();
                }
            }
            return Results.BadRequest(new HttpValidationProblemDetails(errors) { Title = "Validation Failed", Status = 400 });
        }

        logger.LogInformation("{Request}: Validation succeeded.", requestName);
        return await next(context);
    }
}