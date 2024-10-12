using System.Runtime.Serialization;

namespace Messenger.Domain.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base("Model is invalid.")
        {
        }

        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
