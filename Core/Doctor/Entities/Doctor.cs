using Domain.Base;
using Domain.Doctor.Enum;

namespace Core.Doctor.Entities;

public sealed class Doctor: BaseAggregateRoot<Guid>
{

    #region Fields


    public string FullName { get; private set; }

    public string NationalNo { get; private set; }

    public LevelType LevelType { get; private set; }

    public List<DoctorSchedule> WeekLySchedule { get; private set; }

    #endregion Fields


    protected override void SetStateByEvent(IEvent @event)
    {
    }

    protected override void ValidateInvariants()
    {
        //ToDo:
        //throw new NotImplementedException();
    }

}
