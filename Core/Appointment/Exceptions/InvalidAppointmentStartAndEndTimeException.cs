namespace Domain.Appointment.Exceptions;

public sealed class InvalidAppointmentStartAndEndTimeException : Exception
{
    public InvalidAppointmentStartAndEndTimeException()
    {
        
    }
    public InvalidAppointmentStartAndEndTimeException(string message) : base(message) { }
}
