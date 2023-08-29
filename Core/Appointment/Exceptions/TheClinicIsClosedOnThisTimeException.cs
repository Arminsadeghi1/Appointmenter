namespace Domain.Appointment.Exceptions;

public sealed class TheClinicIsClosedOnThisTimeException : Exception
{
    public TheClinicIsClosedOnThisTimeException()
    {
        
    }
    public TheClinicIsClosedOnThisTimeException(string message) : base(message) { }
}
