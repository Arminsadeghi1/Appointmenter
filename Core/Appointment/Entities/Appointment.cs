using Domain;
using Domain.Base;

namespace Core.Appointment.Entities;

public sealed class Appointment : BaseAggregateRoot<Guid>
{
    #region Fields


    public DateTime StartTime { get; private set; }

    public DateTime EndTime { get; private set; }


    public AppointmentDoctor Doctor { get; private set; }

    public AppointmentPatient Patient { get; private set; }

    #endregion Fields


    protected override void SetStateByEvent(IEvent @event)
    {
        throw new NotImplementedException();
    }

    protected override void ValidateInvariants()
    {
        //ToDo:
        //throw new NotImplementedException();
    }

}
