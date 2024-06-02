using System.Runtime.Serialization;

namespace PrescriptionApp.Exceptions;

public class PrescriptionException : Exception
{
    public PrescriptionException()
    {
    }

    protected PrescriptionException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public PrescriptionException(string? message) : base(message)
    {
    }
}