using System.Runtime.Serialization;

namespace Messenger.Domain.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        : base("Unauthorized.")
        {
        }

        public UnauthorizedException(string message)
            : base(message)
        {
        }

        public UnauthorizedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected UnauthorizedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
