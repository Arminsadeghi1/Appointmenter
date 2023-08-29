namespace Domain.Appointment.Exceptions;

public sealed class InvalidDurationMinutesForExpertDoctorException : Exception
{
    public InvalidDurationMinutesForExpertDoctorException()
    {
        
    }
    public InvalidDurationMinutesForExpertDoctorException(string message) : base(message) { }
}
