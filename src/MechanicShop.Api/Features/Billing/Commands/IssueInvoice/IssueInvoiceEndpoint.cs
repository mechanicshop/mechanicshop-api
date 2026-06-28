using Asp.Versioning.Builder;

using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.Billing.Dtos;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.Billing.Commands.IssueInvoice;

public class IssueInvoiceEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapPost("/api/v{version:apiVersion}/invoices/workorders/{workOrderId:guid}", async (Guid workOrderId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new IssueInvoiceCommand(workOrderId), ct);
            return result.Match(
                invoice => Results.Created($"/api/v1/invoices/{invoice.InvoiceId}", invoice),
                error => error.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization("ManagerOnly")
        .WithName("IssueInvoiceForWorkOrder")
        .WithSummary("Issues an invoice for a completed work order.")
        .WithDescription("Creates an invoice for the specified work order and returns the generated invoice.")
        .Produces<InvoiceDto>(StatusCodes.Status201Created)
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}
