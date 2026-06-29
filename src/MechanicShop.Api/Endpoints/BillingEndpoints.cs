using Asp.Versioning.Builder;

using MechanicShop.Api.Extensions;
using MechanicShop.Api.Features.Billing.Commands.IssueInvoice;
using MechanicShop.Api.Features.Billing.Queries.GetInvoiceById;
using MechanicShop.Api.Features.Billing.Queries.GetInvoicePdf;

using MediatR;

namespace MechanicShop.Api.Endpoints;

public static class BillingEndpoints
{
    public static IEndpointRouteBuilder MapBillingEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var group = app.MapGroup("/api/v{version:apiVersion}/invoices")
            .WithApiVersionSet(versionSet)
            .HasApiVersion(1.0)
            .RequireAuthorization("ManagerOnly")
            .MapToApiVersion(1.0);

        group.MapPost("/workorders/{workOrderId:guid}", async (Guid workOrderId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new IssueInvoiceCommand(workOrderId), ct);

            return result.Match(
                invoice => Results.Created($"/api/v1/invoices/{invoice.InvoiceId}", invoice),
                error => error.ToProblem());
        })
        .WithName("IssueInvoiceForWorkOrder");

        group.MapGet("/{invoiceId:guid}", async (Guid invoiceId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetInvoiceByIdQuery(invoiceId), ct);

            return result.Match(Results.Ok, error => error.ToProblem());
        })
        .WithName("GetInvoice");

        group.MapGet("/{invoiceId:guid}/pdf", async (Guid invoiceId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetInvoicePdfQuery(invoiceId), ct);

            return result.Match(
                pdf => Results.File(pdf.Content!, "application/pdf", pdf.FileName),
                error => error.ToProblem());
        })
        .WithName("DownloadInvoicePdf");

        return app;
    }
}
