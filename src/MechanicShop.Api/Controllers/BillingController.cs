using Asp.Versioning;

using MechanicShop.Api.Features.Billing.Commands.IssueInvoice;
using MechanicShop.Api.Features.Billing.Queries.GetInvoiceById;
using MechanicShop.Api.Features.Billing.Queries.GetInvoicePdf;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Controllers;

[Route("api/v{version:apiVersion}/invoices")]
[ApiVersion("1.0")]
[Authorize("ManagerOnly")]
public class BillingController : ApiController
{
    private readonly ISender _sender;

    public BillingController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("workorders/{workOrderId:guid}")]
    public async Task<ActionResult> IssueInvoice(Guid workOrderId, CancellationToken ct)
    {
        var result = await _sender.Send(new IssueInvoiceCommand(workOrderId), ct);

        return result.Match<ActionResult>(
            invoice => Created($"/api/v1/invoices/{invoice.InvoiceId}", invoice),
            Problem);
    }

    [HttpGet("{invoiceId:guid}")]
    public async Task<ActionResult> GetInvoice(Guid invoiceId, CancellationToken ct)
    {
        var result = await _sender.Send(new GetInvoiceByIdQuery(invoiceId), ct);

        return result.Match<ActionResult>(Ok, Problem);
    }

    [HttpGet("{invoiceId:guid}/pdf")]
    public async Task<ActionResult> GetInvoicePdf(Guid invoiceId, CancellationToken ct)
    {
        var result = await _sender.Send(new GetInvoicePdfQuery(invoiceId), ct);

        return result.Match<ActionResult>(
            pdf => File(pdf.Content!, "application/pdf", pdf.FileName),
            Problem);
    }
}
