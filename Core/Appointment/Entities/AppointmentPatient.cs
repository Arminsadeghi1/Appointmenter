using Domain.Base;
using Domain.Doctor.Enum;

namespace Core.Appointment.Entities;

public sealed class AppointmentPatient : BaseValueObject<AppointmentPatient>
{
    #region Fields


    public Guid PatientId { get; private set; }

    public string FullName { get; private set; }



    #endregion Fields


    public override int ObjectGetHashCode()
    {
        throw new NotImplementedException();
    }

    public override bool ObjectIsEqual(AppointmentPatient otherObject)
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }




}
