namespace Cqrs.Template.Application.Behaviors;

public interface IInvalidateCache
{
    public string CacheKeyToInvalidate { get; }
}
