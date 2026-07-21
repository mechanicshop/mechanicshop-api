using Asp.Versioning;

using MechanicShop.Application.Features.Billing.Commands.IssueInvoice;
using MechanicShop.Application.Features.Billing.Queries.GetInvoiceById;
using MechanicShop.Application.Features.Billing.Queries.GetInvoicePdf;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Controllers;

[Route("api/v{version:apiVersion}/invoices")]
[ApiVersion("1.0")]
[Authorize("ManagerOnly")]
public class BillingController(ISender sender) : ApiController
{
    [HttpPost("workorders/{workOrderId:guid}")]
    public async Task<ActionResult> IssueInvoice(Guid workOrderId, CancellationToken ct)
    {
        var result = await sender.Send(new IssueInvoiceCommand(workOrderId), ct);

        return result.Match(
            invoice => Created($"/api/v1/invoices/{invoice.InvoiceId}", invoice),
            Problem);
    }

    [HttpGet("{invoiceId:guid}")]
    public async Task<ActionResult> GetInvoice(Guid invoiceId, CancellationToken ct)
    {
        var result = await sender.Send(new GetInvoiceByIdQuery(invoiceId), ct);

        return result.Match(Ok, Problem);
    }

    [HttpGet("{invoiceId:guid}/pdf")]
    public async Task<ActionResult> GetInvoicePdf(Guid invoiceId, CancellationToken ct)
    {
        var result = await sender.Send(new GetInvoicePdfQuery(invoiceId), ct);

        return result.Match(
            pdf => File(pdf.Content!, "application/pdf", pdf.FileName),
            Problem);
    }
}
