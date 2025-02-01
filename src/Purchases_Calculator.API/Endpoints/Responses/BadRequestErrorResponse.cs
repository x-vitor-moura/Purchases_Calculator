namespace Purchases_Calculator.API.Endpoints.Responses;

public record BadRequestErrorResponse(string Title, 
                                      int Status, 
                                      string Detail, 
                                      PathString Instance);