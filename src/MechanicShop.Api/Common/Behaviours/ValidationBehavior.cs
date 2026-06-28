using MediatR;

namespace MechanicShop.Api.Common.Behaviours;

using FluentValidation;

using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Domain.Common.Results.Abstractions;

public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResult
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (validator is null)
        {
            return await next(ct);
        }

        var validationResult = await validator.ValidateAsync(request, ct);

        if (validationResult.IsValid)
        {
            return await next(ct);
        }

        var errors = validationResult.Errors
            .ConvertAll(error => Error.Validation(
                code: error.PropertyName,
                description: error.ErrorMessage));

        return (dynamic)errors;
    }
}
