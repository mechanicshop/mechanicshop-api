using MechanicShop.Api.Domain.Workorders.Enums;

namespace MechanicShop.Api.Features.Scheduling.Dtos;

public class SpotDto
{
    public Spot Spot { get; set; }
    public List<AvailabilitySlotDto> Slots { get; set; } = [];
}