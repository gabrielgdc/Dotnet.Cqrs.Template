using System;

namespace Cqrs.Template.Application.Behaviors;

public interface ICacheable
{
    public string CacheKey { get; }
    public TimeSpan Expiration { get; }
}
