using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.ApiResonse;

namespace Common.Events
{

    // If don't expect result from task
    public static class ApiEventHandler
    {
        public static async Task<BaseApiResponse<object>> EventHandleAsync(Func<Task> function)
        {
            {
                BaseApiResponse<object> response;

                try
                {
                    await function.Invoke();
                    response = new BaseApiResponse<object>()
                    {
                        Data = default,
                        Errors = null,
                        IsSuccess = true
                    };

                }
                catch (Exception e) { ErrorHandler.Handle(e, out response); };


                return response;
            }
        }

        public static BaseApiResponse<object> EventHandle(Action function)
        {
            BaseApiResponse<object> response;

            try
            {
                function.Invoke();
                response = new BaseApiResponse<object>()
                {
                    Data = default,
                    Errors = null,
                    IsSuccess = true
                };

            }
            catch (Exception e) { ErrorHandler.Handle(e, out response); };


            return response;
        }
    }

    // if expect result from task
    public static class ApiEventHandler<TResult>
    {
        public static async Task<BaseApiResponse<TResult>> EventHandleAsync(Func<Task<TResult>> function)
        {
            BaseApiResponse<TResult> response;

            try
            {
                var result = await function.Invoke();
                response = new BaseApiResponse<TResult>()
                {
                    Data = result,
                    Errors = null,
                    IsSuccess = true
                };

            }
            catch (Exception e) { ErrorHandler.Handle(e, out response); };

            return response;
        }

        public static BaseApiResponse<TResult> EventHandle(Func<TResult> function)
        {
            BaseApiResponse<TResult> response;

            try
            {
                var result = function.Invoke();
                response = new BaseApiResponse<TResult>()
                {
                    Data = result,
                    Errors = null,
                    IsSuccess = true
                };

            }
            catch (Exception e) { ErrorHandler.Handle(e, out response); };

            return response;
        }
    }

    public static class ErrorHandler
    {
        public static void Handle<TResult>(Exception e, out BaseApiResponse<TResult> response)
        {
            if (e is EndpointException)
            {
                response = new()
                {
                    Data = default,
                    Errors = new System.Collections.Generic.List<string>() { e.Message },
                    IsSuccess = false
                };
            }
            else if (e is ExceptionList list)
            {
                response = new()
                {
                    Data = default,
                    Errors = list.ExceptionMessages,
                    IsSuccess = false
                };
            }
            else
            {
                response = new()
                {
                    Data = default,
                    Errors = new System.Collections.Generic.List<string>() { e.Message },
                    IsSuccess = false
                };
            }
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