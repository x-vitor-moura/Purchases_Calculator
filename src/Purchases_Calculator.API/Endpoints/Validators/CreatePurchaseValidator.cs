using FluentValidation;
using Purchases_Calculator.API.Endpoints.Requests;

namespace Purchases_Calculator.API.Endpoints.Validators;

public class CreatePurchaseValidator : AbstractValidator<CreatePurchaseRequest>
{
    public CreatePurchaseValidator()
    {
        // Ensure that exactly one of Net, Gross, or Vat is provided
        RuleFor(r => r)
            .Must(OnlyOneAmountFilled)
            .WithMessage("Exactly one of 'Net', 'Gross', or 'Vat' must be provided.");

        // Net, Gross, and Vat must be > 0 if provided
        RuleFor(r => r.Net)
            .GreaterThan(0).When(r => r.Net.HasValue)
            .WithMessage("'Net' must be greater than 0.");

        RuleFor(r => r.Gross)
            .GreaterThan(0).When(r => r.Gross.HasValue)
            .WithMessage("'Gross' must be greater than 0.");

        RuleFor(r => r.Vat)
            .GreaterThan(0).When(r => r.Vat.HasValue)
            .WithMessage("'Vat' must be greater than 0.");

        // VatRate must be 10, 13, or 20
        RuleFor(r => r.VatRate)
            .Must(v => v == 10 || v == 13 || v == 20)
            .WithMessage("'VatRate' must be 10, 13, or 20.");
    }

    private bool OnlyOneAmountFilled(CreatePurchaseRequest request)
    {
        int count = 0;
        if (request.Net.HasValue) count++;
        if (request.Gross.HasValue) count++;
        if (request.Vat.HasValue) count++;

        return count == 1; // Only one should be filled
    }
}
