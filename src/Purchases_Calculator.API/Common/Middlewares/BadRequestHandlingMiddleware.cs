using Purchases_Calculator.API.Endpoints.Responses;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Purchases_Calculator.API.Common.Middlewares;

public class BadRequestHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<BadRequestHandlingMiddleware> _logger;

    // Dictionary for custom error messages
    private static readonly Dictionary<string, string> FieldErrorMessages = new()
    {
        { "Net", "'Net' must be numeric and greater the 0." },
        { "Gross", "'Gross' must be numeric and greater the 0." },
        { "Vat", "'VAT' must be numeric and greater the 0." },
        { "VatRate", "'VAT Rate' must be numeric and must be 10, 13, or 20." }
    };

    public BadRequestHandlingMiddleware(RequestDelegate next, ILogger<BadRequestHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BadHttpRequestException ex)
        {
            string fieldName = ExtractFieldName(ex.InnerException as JsonException);
            string errorMessage = GetCustomErrorMessage(fieldName);

            _logger.LogError(ex, "errorMessage");

            var errorResponse = new BadRequestErrorResponse("Invalid JSON request",
                                                            400,
                                                            errorMessage,
                                                            context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }

    public static string ExtractFieldName(JsonException? jsonException)
    {
        if (jsonException == null || string.IsNullOrEmpty(jsonException.Message))
        {
            return "unknown";
        }

        // Extracts the JSON field name from the error message (e.g., "Path: $.net")
        Match match = Regex.Match(jsonException.Message, @"Path: \$\.(\w+)");
        return match.Success ? match.Groups[1].Value : "unknown";
    }

    public static string GetCustomErrorMessage(string fieldName)
    {
        return FieldErrorMessages.TryGetValue(fieldName, out string message)
            ? message
            : $"'{fieldName}' contains an invalid value.";
    }
}