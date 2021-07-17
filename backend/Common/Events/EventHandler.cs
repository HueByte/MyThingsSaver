using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Common.ApiResonse;

namespace Common.Events
{
    public class ApiEventHandler<TResult>
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
            catch (Exception e)
            {
                response = new BaseApiResponse<TResult>()
                {
                    Data = default,
                    Errors = new System.Collections.Generic.List<string>() { e.Message },
                    IsSuccess = false
                };
            }

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
            catch (Exception e)
            {
                response = new BaseApiResponse<TResult>()
                {
                    Data = default,
                    Errors = new System.Collections.Generic.List<string>() { e.Message },
                    IsSuccess = false
                };
            }

            return response;
        }
    }
}