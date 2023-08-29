using Domain.Base;
using Domain.Doctor.Enum;

namespace Core.Appointment.Entities;

public sealed class AppointmentDoctor : BaseValueObject<AppointmentDoctor>
{
    #region Fields

    public Guid DoctorId { get; private set; }

    public string FullName { get; private set; }

    public LevelType LevelType { get; private set; }



    #endregion Fields



    public override int ObjectGetHashCode()
    {
        throw new NotImplementedException();
    }

    public override bool ObjectIsEqual(AppointmentDoctor otherObject)
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }


}
