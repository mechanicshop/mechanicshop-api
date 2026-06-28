namespace MechanicShop.Api.DTOs.Responses;

public sealed record OperatingHoursResponse(TimeOnly OpeningTime, TimeOnly ClosingTime);