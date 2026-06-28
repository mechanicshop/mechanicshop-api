using FluentValidation;

namespace MechanicShop.Api.Features.Billing.Commands.IssueInvoice;

public sealed class IssueInvoiceCommandValidator : AbstractValidator<IssueInvoiceCommand>
{
    public IssueInvoiceCommandValidator()
    {
        RuleFor(request => request.WorkOrderId)
            .NotEmpty()
            .WithErrorCode("WorkOrderId_Is_Required")
            .WithMessage("WorkOrderId is required.");
    }
}
