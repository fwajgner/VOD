namespace API.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class VodApiException : Exception
    {
        public VodApiException()
        {

        }

        public VodApiException(string message) : base(message)
        {

        }

        public VodApiException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public VodApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
