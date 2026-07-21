using MechanicShop.Domain.Common.Results;

namespace MechanicShop.Domain.Common;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
    DateTimeOffset? DeletedAtUtc { get; set; }
    Result<Deleted> Delete(DateTimeOffset deletedAtUtc);
}
