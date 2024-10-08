using System.Runtime.Serialization;

namespace Messenger.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        : base("Requested resource was not found.")
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
