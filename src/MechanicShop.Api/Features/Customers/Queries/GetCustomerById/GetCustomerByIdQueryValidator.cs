using FluentValidation;

namespace MechanicShop.Api.Features.Customers.Queries.GetCustomerById;

public sealed class GetCustomerByIdQueryValidator : AbstractValidator<GetCustomerByIdQuery>
{
    public GetCustomerByIdQueryValidator()
    {
        RuleFor(request => request.CustomerId)
            .NotEmpty()
            .WithErrorCode("CustomerId_Is_Required")
            .WithMessage("CustomerId is required.");
    }
}
