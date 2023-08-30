namespace Domain.Appointment.Exceptions;

public sealed class NotFoundAnyAppiontmentChanseException : Exception
{
    public NotFoundAnyAppiontmentChanseException()
    {
        
    }
    public NotFoundAnyAppiontmentChanseException(string message) : base(message) { }
}
