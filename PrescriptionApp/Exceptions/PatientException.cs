using System.Runtime.Serialization;

namespace PrescriptionApp.Exceptions;

public class PatientException : Exception
{
    public PatientException()
    {
    }

    protected PatientException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public PatientException(string? message) : base(message)
    {
    }
}