﻿namespace Messenger.Domain.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base("You do not have permission to perform this action.")
        {
        }

        public ForbiddenException(string message) : base(message)
        {
        }

        public ForbiddenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
