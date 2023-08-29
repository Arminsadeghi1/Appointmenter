namespace Domain.Appointment.Exceptions;

public sealed class InvalidNumberOfPatientAppointmentsPerDayException : Exception
{
    public InvalidNumberOfPatientAppointmentsPerDayException()
    {
        
    }
    public InvalidNumberOfPatientAppointmentsPerDayException(string message) : base(message) { }
}
