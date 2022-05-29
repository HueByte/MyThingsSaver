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
            switch (e)
            {
                case EndpointException:
                    response = new()
                    {
                        Data = default,
                        Errors = new System.Collections.Generic.List<string>() { e.Message },
                        IsSuccess = false
                    };
                    break;

                case ExceptionList list:
                    response = new()
                    {
                        Data = default,
                        Errors = list.ExceptionMessages,
                        IsSuccess = false
                    };
                    break;
                default:
                    response = new()
                    {
                        Data = default,
                        Errors = new System.Collections.Generic.List<string>() { e.Message },
                        IsSuccess = false
                    };
                    break;
            }

            return Task.CompletedTask;
        }
    }

    public class ExceptionList : Exception
    {
        public List<string> ExceptionMessages { get; set; }
        public ExceptionList(List<string> exceptionMessages)
        {
            ExceptionMessages = exceptionMessages;
        }
    }

    public class EndpointException : Exception
    {
        public EndpointException(string ExceptionMessage) : base(ExceptionMessage) { }
    }
}