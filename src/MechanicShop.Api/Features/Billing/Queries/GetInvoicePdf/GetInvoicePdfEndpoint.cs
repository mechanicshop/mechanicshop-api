using Asp.Versioning.Builder;

using MechanicShop.Api.Endpoints;
using MechanicShop.Api.Extensions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Features.Billing.Queries.GetInvoicePdf;

public class GetInvoicePdfEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app, ApiVersionSet apiVersionSet)
    {
        app.MapGet("/api/v{version:apiVersion}/invoices/{invoiceId:guid}/pdf", async (Guid invoiceId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetInvoicePdfQuery(invoiceId), ct);
            return result.Match(
                pdf => Results.File(pdf.Content!, "application/pdf", pdf.FileName),
                error => error.ToProblem());
        })
        .WithApiVersionSet(apiVersionSet)
        .HasApiVersion(1.0)
        .RequireAuthorization("ManagerOnly")
        .WithName("DownloadInvoicePdf")
        .WithSummary("Downloads the invoice as a PDF.")
        .WithDescription("Returns the PDF file associated with the given invoice ID.")
        .Produces(StatusCodes.Status200OK, contentType: "application/pdf")
        .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
        .MapToApiVersion(1.0);
    }
}
