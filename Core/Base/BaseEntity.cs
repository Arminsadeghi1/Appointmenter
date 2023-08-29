using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Base;


public abstract class BaseEntity<TId> where TId : IEquatable<TId>
{
    private readonly List<IEvent> _events;

    private Action<IEvent> _applier;

    private bool IsLastEvent => _events.Count() == 1;

    public TId Id { get; protected set; }

    protected BaseEntity(Action<IEvent> applier)
    {
        _events = new List<IEvent>();
        _applier = applier;
    }

    protected BaseEntity()
    {
        _events = new List<IEvent>();
    }

    public void HandleEvent(IEvent @event)
    {
        _events.Add(@event);
        SetStateByEvent(@event);
        if (IsLastEvent)
        {
            ValidateInvariants();
        }

        _applier(@event);
        _events.Remove(@event);
    }

    public void SetApplier(Action<IEvent> applier)
    {
        _applier = applier;
    }

    protected abstract void SetStateByEvent(IEvent @event);

    protected abstract void ValidateInvariants();

}