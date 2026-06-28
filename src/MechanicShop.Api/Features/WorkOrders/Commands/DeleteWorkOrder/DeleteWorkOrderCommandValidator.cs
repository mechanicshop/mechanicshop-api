using FluentValidation;

namespace MechanicShop.Api.Features.WorkOrders.Commands.DeleteWorkOrder;

public sealed class DeleteWorkOrderCommandValidator : AbstractValidator<DeleteWorkOrderCommand>
{
    public DeleteWorkOrderCommandValidator()
    {
        RuleFor(x => x.WorkOrderId)
           .NotEmpty()
           .WithErrorCode("WorkOrderId_Required")
           .WithMessage("WorkOrderId is required.");
    }
}
