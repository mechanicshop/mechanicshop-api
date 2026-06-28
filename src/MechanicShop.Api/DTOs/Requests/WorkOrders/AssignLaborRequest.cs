using System.ComponentModel.DataAnnotations;

namespace MechanicShop.Api.DTOs.Requests.WorkOrders;

public class AssignLaborRequest
{
    [Required(ErrorMessage = "LaborId is required.")]
    public string LaborId { get; init; } = string.Empty;
}