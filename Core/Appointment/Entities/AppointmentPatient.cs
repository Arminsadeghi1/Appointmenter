using Domain.Base;
using Domain.Doctor.Enum;

namespace Core.Appointment.Entities;

public class AppointmentPatient : BaseValueObject<AppointmentPatient>
{

    private AppointmentPatient()
    {
        
    }
    private AppointmentPatient(Guid patientId, string fullName)
    {
        PatientId = patientId;
        FullName = fullName;
    }

    public static AppointmentPatient New(Guid patientId, string fullName)
    {
        return new AppointmentPatient(patientId, fullName);
    }

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
