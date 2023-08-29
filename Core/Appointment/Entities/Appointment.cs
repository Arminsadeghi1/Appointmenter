using Domain;
using Domain.Appointment.Events;
using Domain.Base;

namespace Core.Appointment.Entities;

public sealed class Appointment : BaseAggregateRoot<Guid>
{
    private Appointment()
    {
    }
    public Appointment(DateTime startDateTime, DateTime endDateTime, AppointmentDoctor doctor, AppointmentPatient patient)
    {
        HandleEvent(new AppointmentCreated()
        {
            StartDateTime = startDateTime,
            EndDateTime = endDateTime,
            Patient = patient,
            Doctor = doctor
        });
    }
    #region Fields


    public DateTime StartDateTime { get; private set; }

    public DateTime EndDateTime { get; private set; }


    public AppointmentDoctor Doctor { get; private set; }

    public AppointmentPatient Patient { get; private set; }

    #endregion Fields


    protected override void SetStateByEvent(IEvent @event)
    {
        switch (@event)
        {
            case AppointmentCreated ev:
                StartDateTime = ev.StartDateTime;
                EndDateTime = ev.EndDateTime;
                Patient = ev.Patient;
                Doctor = ev.Doctor;
                break;
        }
    }

    protected override void ValidateInvariants()
    {
        //ToDo:
        //throw new NotImplementedException();
    }

}
