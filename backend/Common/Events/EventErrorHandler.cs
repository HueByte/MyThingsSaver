using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.ApiResonse;

namespace Common.Events
{
    public static class EventErrorHandler
    {
        public static Task Handle<TResult>(Exception e, out BaseApiResponse<TResult> response)
        {
            response = e switch
            {
                EndpointException => new()
                {
                    Data = default,
                    Errors = new System.Collections.Generic.List<string>() { e.Message },
                    IsSuccess = false
                },
                EndpointExceptionList list => new()
                {
                    Data = default,
                    Errors = list.ExceptionMessages,
                    IsSuccess = false
                },
                _ => new()
                {
                    Data = default,
                    Errors = new System.Collections.Generic.List<string>() { e.Message },
                    IsSuccess = false
                },
            };

            return Task.CompletedTask;
        }
    }

    public class EndpointExceptionList : Exception
    {
        public List<string> ExceptionMessages { get; set; }
        public EndpointExceptionList(List<string> exceptionMessages)
        {
            ExceptionMessages = exceptionMessages;
        }
    }

    public class EndpointException : Exception
    {
        public EndpointException(string ExceptionMessage) : base(ExceptionMessage) { }
    }
}