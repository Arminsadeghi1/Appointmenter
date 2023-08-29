namespace Domain.Appointment.Exceptions;

public sealed class DoctorIsNotAvailableOnThisDayException : Exception
{
    public DoctorIsNotAvailableOnThisDayException()
    {
        
    }
    public DoctorIsNotAvailableOnThisDayException(string message) : base(message) { }
}
