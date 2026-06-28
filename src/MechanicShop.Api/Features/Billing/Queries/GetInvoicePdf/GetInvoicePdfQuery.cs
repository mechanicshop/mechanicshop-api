using MechanicShop.Api.Domain.Common.Results;
using MechanicShop.Api.Features.Billing.Dtos;

using MediatR;

namespace MechanicShop.Api.Features.Billing.Queries.GetInvoicePdf;

public sealed record GetInvoicePdfQuery(Guid InvoiceId) : IRequest<Result<InvoicePdfDto>>
{
}