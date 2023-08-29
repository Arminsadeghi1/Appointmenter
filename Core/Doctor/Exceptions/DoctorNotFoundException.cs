using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Doctor.Exceptions;

public sealed class DoctorNotFoundException : Exception
{
    public DoctorNotFoundException()
    {
        
    }
    public DoctorNotFoundException(string message) : base(message) { }
}
