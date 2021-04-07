using System;

namespace Pds.Api.Contracts.Exceptions
{
    public class ExceptionResponse
    {
        public ExceptionResponse(Exception exception)
        {
            Exception = exception.ToString();
            Message = exception.Message;
            StackTrace = exception.StackTrace;
            if (exception.InnerException is not null)
                InnerException = new ExceptionResponse(exception.InnerException);
        }

        public string Exception { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public ExceptionResponse InnerException { get; set; }
    }
}