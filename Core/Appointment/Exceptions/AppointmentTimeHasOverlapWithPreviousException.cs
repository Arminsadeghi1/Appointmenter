namespace Domain.Appointment.Exceptions;

public sealed class AppointmentTimeHasOverlapWithPreviousException : Exception
{
    public AppointmentTimeHasOverlapWithPreviousException()
    {
        
    }
    public AppointmentTimeHasOverlapWithPreviousException(string message) : base(message) { }
}
