using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cqrs.Template.Domain.Enums;
using Cqrs.Template.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Cqrs.Template.Application.Behaviors;

public class PipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IMediator _bus;
    private readonly IMemoryCache _cache;
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<PipelineBehavior<TRequest, TResponse>> _logger;

    public PipelineBehavior(IMediator bus, IMemoryCache cache, IEnumerable<IValidator<TRequest>> validators, ILogger<PipelineBehavior<TRequest, TResponse>> logger)
    {
        _bus = bus;
        _cache = cache;
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!ValidateRequest(request))
        {
            return default;
        }

        if (request is IInvalidateCache invalidateCacheRequest)
        {
            _cache.Remove(invalidateCacheRequest.CacheKeyToInvalidate);
            _logger.LogInformation("Invalidated Cache Key {CacheKeyToInvalidate}", invalidateCacheRequest.CacheKeyToInvalidate);
            return await next();
        }

        if (request is not ICacheable cacheableRequest) return await next();

        _logger.LogInformation("Cached Request {CacheKey} for {Expiration}", cacheableRequest.CacheKey, cacheableRequest.Expiration.ToString());
        return await _cache.GetOrCreateAsync(cacheableRequest.CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = cacheableRequest.Expiration;
            return await next();
        });
    }

    private bool ValidateRequest(TRequest request)
    {
        var failures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (!failures.Any()) return true;

        foreach (var error in failures)
        {
            _bus.Publish(new ExceptionNotification(error.ErrorCode, error.ErrorMessage, ExceptionType.Client, error.PropertyName));
        }

        return false;
    }
}
