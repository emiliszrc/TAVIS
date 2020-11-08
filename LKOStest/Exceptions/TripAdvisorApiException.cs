using System;
using System.Runtime.Serialization;

namespace LKOStest.Exceptions
{
    [Serializable]
    internal class TripAdvisorApiException : Exception
    {
        public TripAdvisorApiException()
        {
        }

        public TripAdvisorApiException(string message) : base(message)
        {
        }

        public TripAdvisorApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TripAdvisorApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}