using MechanicShop.Api.Common.Interfaces;
using MechanicShop.Api.Domain.Common.Results.Abstractions;

using MediatR;

using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

using IResult = MechanicShop.Api.Domain.Common.Results.Abstractions.IResult;

namespace MechanicShop.Api.Common.Behaviours;

public class CachingBehavior<TRequest, TResponse>(
    HybridCache cache,
    ILogger<CachingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (request is not ICachedQuery cachedRequest)
        {
            return await next(ct);
        }

        logger.LogInformation("Checking cache for {RequestName}", typeof(TRequest).Name);

        var result = await cache.GetOrCreateAsync<TResponse>(
            cachedRequest.CacheKey,
            _ => new ValueTask<TResponse>((TResponse)(object)null!),
            new HybridCacheEntryOptions
            {
                Flags = HybridCacheEntryFlags.DisableUnderlyingData
            },
            cancellationToken: ct);

        if (result is null)
        {
            result = await next(ct);

            if (result is IResult res && res.IsSuccess)
            {
                logger.LogInformation("Caching result for {RequestName}", typeof(TRequest).Name);

                await cache.SetAsync(
                    cachedRequest.CacheKey,
                    result,
                    new HybridCacheEntryOptions
                    {
                        Expiration = cachedRequest.Expiration
                    },
                    cachedRequest.Tags,
                    ct);
            }
        }

        return result;
    }
}