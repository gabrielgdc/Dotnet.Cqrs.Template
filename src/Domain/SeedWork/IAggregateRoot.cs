namespace Domain.SeedWork;

/// <summary>
/// Represents the root entity within an aggregate in the Domain-Driven Design (DDD) pattern.
/// </summary>
/// <remarks>
/// An aggregate root manages the lifecycle of its related entities (child objects) and enforces domain invariants
/// within the aggregate. It serves as the entry point for interactions with the aggregate from external contexts.
/// </remarks>
public interface IAggregateRoot;