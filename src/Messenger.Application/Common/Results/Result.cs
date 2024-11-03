namespace Messenger.Application.Common.Results
{
    public class Result
    {
        protected Result() { }

        public string Message { get; protected set; }

        public bool Succeeded { get; protected set; }

        public static Result Fail() 
            => new Result { Succeeded = false };

        public static Result Fail(string message) 
            => new Result { Succeeded = false, Message = message };

        public static Result Success() 
            => new Result { Succeeded = true };

        public static Result Success(string message) 
            => new Result { Succeeded = true, Message = message };
    }

    public class Result<T> : Result
    {
        protected Result() { }

        public T Data { get; private init; }

        public new static Result<T> Fail() 
            => (Result<T>)Result.Fail();

        public new static Result<T> Fail(string message) 
            => (Result<T>)Result.Fail(message);

        public new static Result<T> Success() 
            => (Result<T>)Result.Success();

        public static Result<T> Success(T data) 
            => new Result<T> { Succeeded = true, Data = data };

        public static Result<T> Success(T data, string message) 
            => new Result<T> { Succeeded = true, Data = data, Message = message };
    }
}
