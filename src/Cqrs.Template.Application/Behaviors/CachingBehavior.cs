using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Cqrs.Template.Application.Behaviors;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

    public CachingBehavior(IMemoryCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trying to retrieve or invalidate a cached response for request {Request}", request.GetType().Name);

        if (request is IInvalidateCache invalidateCacheRequest)
        {
            _cache.Remove(invalidateCacheRequest.CacheKeyToInvalidate);
            _logger.LogInformation("Request cache invalidated with the key {CacheKeyToInvalidate}", invalidateCacheRequest.CacheKeyToInvalidate);
            return await next();
        }

        if (request is not ICacheable cacheableRequest) return await next();

        _logger.LogInformation("Request cached with the key {CacheKey} for {Expiration} minutes", cacheableRequest.CacheKey, cacheableRequest.Expiration.TotalMinutes);
        return await _cache.GetOrCreateAsync(cacheableRequest.CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = cacheableRequest.Expiration;
            _logger.LogInformation("Returning cached response for request {Request}", request.GetType().Name);
            return await next();
        });
    }
}
