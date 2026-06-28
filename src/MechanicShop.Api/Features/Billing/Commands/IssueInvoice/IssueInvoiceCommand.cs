using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Features.Billing.Dtos;

using MediatR;

namespace MechanicShop.Api.Features.Billing.Commands.IssueInvoice;

public sealed record IssueInvoiceCommand(Guid WorkOrderId) : IRequest<Result<InvoiceDto>>;