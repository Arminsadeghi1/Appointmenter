using Core.Appointment.Entities;
using Domain.Base;

namespace Domain.Appointment.Events;

public sealed class AppointmentCreated : IEvent
{

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }


    public AppointmentDoctor Doctor { get; set; }

    public AppointmentPatient Patient { get; set; }

}
