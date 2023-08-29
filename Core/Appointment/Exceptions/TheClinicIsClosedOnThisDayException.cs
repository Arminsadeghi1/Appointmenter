namespace Domain.Appointment.Exceptions;

public sealed class TheClinicIsClosedOnThisDayException : Exception
{
    public TheClinicIsClosedOnThisDayException()
    {
        
    }
    public TheClinicIsClosedOnThisDayException(string message) : base(message) { }
}
