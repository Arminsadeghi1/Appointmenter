namespace Domain.Appointment.Commands;

public sealed class SetEarliestAppointmentCommand
{

    public Guid DoctorId { get; set; }

    public Guid PatientId { get; set; }

    public byte DurationMinutes { get; set;}
}
