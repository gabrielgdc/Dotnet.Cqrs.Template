using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cqrs.Template.Domain.Enums;
using Cqrs.Template.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Cqrs.Template.Application.Behaviors;

public class PipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IMemoryCache _cache;
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IMediator _bus;

    public PipelineBehavior(IMemoryCache cache, IEnumerable<IValidator<TRequest>> validators, IMediator bus)
    {
        _cache = cache;
        _validators = validators;
        _bus = bus;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!ValidateRequest(request))
        {
            return default;
        }

        if (request is not IProvideCacheKey cacheableRequest) return await next();

        var cacheKey = cacheableRequest.CacheKey;

        if (_cache.TryGetValue<TResponse>(cacheKey, out var cacheResponse))
        {
            return cacheResponse;
        }

        var response = await next();
        _cache.Set(cacheKey, response, TimeSpan.FromMinutes(1));

        return response;
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
