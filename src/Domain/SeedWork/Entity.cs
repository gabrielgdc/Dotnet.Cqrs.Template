using System;
using System.Collections.Generic;
using MediatR;

namespace Domain.SeedWork;

/// <summary>
/// Represents a base class for domain entities within the Domain-Driven Design (DDD) pattern.
/// </summary>
/// <remarks>
/// This abstract class provides core functionality for domain entities, including:
///   - Unique identifier (GUID) generation on object creation.
///   - Equality comparison based on ID for persistent entities.
///   - Overridden methods for Equals(), GetHashCode(), and equality operators (== and !=).
///   - Tracking and management of domain events through an INotification collection.
/// Subclasses of this class should represent specific domain concepts within your application.
/// </remarks>
public abstract class Entity
{
    /// <summary>
    /// The unique identifier for the entity.
    /// </summary>
    public virtual Guid Id { get; } = Guid.NewGuid();

    #region Entity

    /// <summary>
    /// Determines whether the entity is considered transient (newly created, not yet persisted).
    /// </summary>
    /// <returns>True if the entity's ID is the default GUID, false otherwise.</returns>
    private bool IsTransient()
    {
        return Id == default;
    }

    /// <summary>
    /// Compares the current object with another object for equality.
    /// </summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns>True if the objects are equal, false otherwise.</returns>
    public override bool Equals(object obj)
    {
        if (obj is not Entity item)
            return false;

        if (ReferenceEquals(this, item))
            return true;

        if (GetType() != item.GetType())
            return false;

        if (item.IsTransient() || IsTransient())
            return false;

        return item.Id == this.Id;
    }

    /// <summary>
    /// Generates a hash code for the current object.
    /// </summary>
    /// <returns>An integer hash code value.</returns>
    public override int GetHashCode()
    {
        if (IsTransient()) return default;

        return Id.GetHashCode() ^ 31;
    }
    
    /// <summary>
    /// Overloaded equality operator for entity comparison.
    /// </summary>
    /// <param name="left">The left-hand side entity in the comparison.</param>
    /// <param name="right">The right-hand side entity in the comparison.</param>
    /// <returns>True if the entities are equal, false otherwise.</returns>
    public static bool operator ==(Entity left, Entity right)
    {
        return left?.Equals(right) ?? Equals(right, null);
    }

    /// <summary>
    /// Overloaded inequality operator for entity comparison.
    /// </summary>
    /// <param name="left">The left-hand side entity in the comparison.</param>
    /// <param name="right">The right-hand side entity in the comparison.</param>
    /// <returns>True if the entities are not equal, false otherwise.</returns>
    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

    #endregion

    #region DomainEvents

    /// <summary>
    /// The collection of domain events associated with the entity.
    /// </summary>
    private List<INotification> _domainEvents;
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

    /// <summary>
    /// Adds a domain event to the entity's event collection.
    /// </summary>
    /// <param name="eventItem">The domain event notification to be added.</param>
    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents ??= new List<INotification>();
        _domainEvents.Add(eventItem);
    }

    /// <summary>
    /// Removes a specific domain event from the entity's event collection.
    /// </summary>
    /// <param name="eventItem">The domain event notification to be removed.</param>
    public void RemoveDomainEvent(INotification eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }

    /// <summary>
    /// Clears all domain events from the entity's event collection.
    /// </summary>
    public void ClearDomainEvent()
    {
        _domainEvents?.Clear();
    }

    #endregion
}
