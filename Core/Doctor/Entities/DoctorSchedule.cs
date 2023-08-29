using Domain.Base;

namespace Core.Doctor.Entities;

public sealed class DoctorSchedule : BaseEntity<Guid>
{

    #region Fields

    public TimeSpan StartTime { get; private set; }

    public TimeSpan EndTime { get; private set; }

    public DayOfWeek DayOfWeek { get; private set; }

    #endregion Fields


    protected override void SetStateByEvent(IEvent @event)
    {
        throw new NotImplementedException();
    }

    protected override void ValidateInvariants()
    {
        throw new NotImplementedException();
    }


}
