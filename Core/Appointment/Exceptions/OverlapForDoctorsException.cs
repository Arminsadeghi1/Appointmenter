namespace Domain.Appointment.Exceptions;

public sealed class OverlapForDoctorsException : Exception
{
    public OverlapForDoctorsException()
    {
        
    }
    public OverlapForDoctorsException(string message) : base(message) { }
}
