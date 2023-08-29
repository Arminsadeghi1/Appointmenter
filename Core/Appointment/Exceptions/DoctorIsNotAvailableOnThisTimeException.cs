namespace Domain.Appointment.Exceptions;

public sealed class DoctorIsNotAvailableOnThisTimeException : Exception
{
    public DoctorIsNotAvailableOnThisTimeException()
    {
        
    }
    public DoctorIsNotAvailableOnThisTimeException(string message) : base(message) { }
}
