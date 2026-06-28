using MechanicShop.Api.Domain.Common.Results;

using MediatR;

namespace MechanicShop.Api.Features.Billing.Commands.SettleInvoice;

public sealed record SettleInvoiceCommand(Guid InvoiceId) : IRequest<Result<Success>>;