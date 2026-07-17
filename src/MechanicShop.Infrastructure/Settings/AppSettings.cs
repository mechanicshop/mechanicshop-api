using System.Diagnostics.CodeAnalysis;

namespace MechanicShop.Infrastructure.Settings;

public class AppSettings
{
    public const string SectionName = "AppSettings";

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
    public CorsSettings Cors { get; init; } = new();
}

public class CorsSettings
{
    [SetsRequiredMembers]
    public CorsSettings()
    {
    }

    public required string PolicyName { get; set; }

    public string[] AllowedOrigins { get; set; } = Array.Empty<string>();
}
