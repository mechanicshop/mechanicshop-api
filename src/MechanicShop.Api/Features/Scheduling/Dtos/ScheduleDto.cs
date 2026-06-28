namespace MechanicShop.Api.Features.Scheduling.Dtos;

public class ScheduleDto
{
    public DateOnly OnDate { get; set; }
    public bool EndOfDay { get; set; }
    public List<SpotDto> Spots { get; set; } = [];
}