using System.ComponentModel.DataAnnotations;

namespace MechanicShop.Api.DTOs.Requests.Customers;

public class CreateVehicleRequest
{
    [Required(ErrorMessage = "Make is required.")]
    public string Make { get; init; } = string.Empty;

    [Required(ErrorMessage = "Model is required.")]
    public string Model { get; init; } = string.Empty;

    [Required(ErrorMessage = "Year is required.")]
    public int Year { get; init; }

    [Required(ErrorMessage = "Spot is required.")]
    public string LicensePlate { get; init; } = string.Empty;
}