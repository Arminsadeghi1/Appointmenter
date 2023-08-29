using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Appointment.Commands;

public sealed class SetAppointmentCommand
{
    public Guid DoctorId { get; set; }

    public Guid PatientId { get; set; }

    public byte DurationMinutes { get; set;}

    public DateTime AppointmentStartDateTime { get; set;}
}
