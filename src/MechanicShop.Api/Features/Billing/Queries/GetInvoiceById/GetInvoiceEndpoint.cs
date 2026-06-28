using Asp.Versioning.Builder;

using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.Billing.Dtos;
using MechanicShop.Api.Features.Billing.Queries.GetInvoiceById;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.Billing.Queries.GetInvoiceById;

public class GetInvoiceEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapGet("/api/v{version:apiVersion}/invoices/{invoiceId:guid}", async (Guid invoiceId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetInvoiceByIdQuery(invoiceId), ct);
            return result.Match(Results.Ok, error => error.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization("ManagerOnly")
        .WithName("GetInvoice")
        .WithSummary("Retrieves an invoice by ID.")
        .WithDescription("Returns detailed information about a specific invoice.")
        .Produces<InvoiceDto>()
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}