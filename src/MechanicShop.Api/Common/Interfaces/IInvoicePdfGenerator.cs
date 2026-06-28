using MechanicShop.Api.Domain.Workorders.Billing;

namespace MechanicShop.Api.Common.Interfaces;

public interface IInvoicePdfGenerator
{
    byte[] Generate(Invoice invoice);
}
