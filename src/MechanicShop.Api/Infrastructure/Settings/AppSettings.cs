namespace MechanicShop.Api.Infrastructure.Settings;

public class AppSettings
{
    public TimeOnly OpeningTime { get; set; }
    public TimeOnly ClosingTime { get; set; }
    public int MaxSpots { get; init; }
    public int MinimumAppointmentDurationInMinutes { get; init; }
    public int LocalCacheExpirationInMins { get; init; }
    public int DistributedCacheExpirationMins { get; init; }
    public int DefaultPageNumber { get; init; }
    public int DefaultPageSize { get; init; }
    public int BookingCancellationThresholdMinutes { get; init; }
    public int OverdueBookingCleanupFrequencyMinutes { get; init; }
    public string CorsPolicyName { get; init; } = null!;
    public string[] AllowedOrigins { get; init; } = null!;
}