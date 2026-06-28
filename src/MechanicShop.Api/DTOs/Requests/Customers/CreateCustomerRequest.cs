using System.ComponentModel.DataAnnotations;

namespace MechanicShop.Api.DTOs.Requests.Customers
{
    public class CreateCustomerRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; init; } = string.Empty;

        [Required(ErrorMessage = "PhoneNumber is required.")]
        [RegularExpression(@"^\+?\d{7,15}$", ErrorMessage = "Phone number must be 7–15 digits and may start with '+'.")]
        public string PhoneNumber { get; init; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email is invalid.")]
        public string Email { get; init; } = string.Empty;

        [MinLength(1, ErrorMessage = "At least one vehicle is required.")]
        public List<CreateVehicleRequest> Vehicles { get; init; } = [];
    }
}