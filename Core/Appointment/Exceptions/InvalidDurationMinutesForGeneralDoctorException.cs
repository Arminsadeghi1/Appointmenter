using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Appointment.Exceptions;

public sealed class InvalidDurationMinutesForGeneralDoctorException : Exception
{
    public InvalidDurationMinutesForGeneralDoctorException()
    {
        
    }
    public InvalidDurationMinutesForGeneralDoctorException(string message) : base(message) { }
}
