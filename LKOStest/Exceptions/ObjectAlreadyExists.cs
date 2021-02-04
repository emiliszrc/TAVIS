using System;
using System.Runtime.Serialization;

namespace LKOStest.Services
{
    public class ObjectAlreadyExists : Exception
    {
        public ObjectAlreadyExists()
        {
        }

        public ObjectAlreadyExists(string message) : base(message)
        {
        }

        public ObjectAlreadyExists(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ObjectAlreadyExists(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}