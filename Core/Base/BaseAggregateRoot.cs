using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Base;

public abstract class BaseAggregateRoot<TId> where TId : IEquatable<TId>
{
    private readonly List<IEvent> _events;

    public TId Id { get; protected set; }

    protected BaseAggregateRoot()
    {
        _events = new List<IEvent>();
    }

    protected void HandleEvent(IEvent @event)
    {
        SetStateByEvent(@event);
        ValidateInvariants();
        _events.Add(@event);
    }

    protected abstract void SetStateByEvent(IEvent @event);

    public IEnumerable<IEvent> GetEvents()
    {
        return _events.AsEnumerable();
    }

    public void ClearEvents()
    {
        _events.Clear();
    }

    protected abstract void ValidateInvariants();

}
