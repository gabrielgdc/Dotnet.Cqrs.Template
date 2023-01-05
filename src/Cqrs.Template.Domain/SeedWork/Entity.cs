using System;
using System.Collections.Generic;
using MediatR;

namespace Cqrs.Template.Domain.SeedWork;

public class Entity
{
    private int? _requestedHashCode;
    private Guid _id;

    public virtual Guid Id => _id;

    protected void SetId()
    {
        _id = Guid.NewGuid();
    }

    #region Entity

    public bool IsTransient()
    {
        return Id == default;
    }

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

    public override int GetHashCode()
    {
        if (IsTransient()) return base.GetHashCode();

        _requestedHashCode ??= Id.GetHashCode() ^ 31;

        return _requestedHashCode.Value;
    }

    public static bool operator ==(Entity left, Entity right)
    {
        return left?.Equals(right) ?? Equals(right, null);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }

    #endregion

    #region domain events

    private List<INotification> _domainEvents;
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents ??= new List<INotification>();
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(INotification eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvent()
    {
        _domainEvents?.Clear();
    }

    #endregion
}
