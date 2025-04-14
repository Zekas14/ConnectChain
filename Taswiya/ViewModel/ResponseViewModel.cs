using Microsoft.AspNetCore.Http;
using ConnectChain.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ConnectChain.ViewModel
{
    public class ResponseViewModel<T> : IActionResult
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public ErrorCode ErrorCode { get; set; }
        public string Message { get; set; }

        

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = "application/json";

            response.StatusCode = IsSuccess ? StatusCodes.Status200OK : GetStatusCode(ErrorCode);

            var result = JsonSerializer.Serialize(this, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await response.WriteAsync(result);
        }

        private int GetStatusCode(ErrorCode errorCode)
        {
            return errorCode switch
            {
                ErrorCode.NotFound => StatusCodes.Status404NotFound,
                ErrorCode.EmailAlreadyConfirmed => StatusCodes.Status409Conflict,
                ErrorCode.UnAuthorized => StatusCodes.Status401Unauthorized,

                _ => StatusCodes.Status400BadRequest
            };
        }
    }

    public class SuccessResponseViewModel<T> : ResponseViewModel<T>
    {
        public SuccessResponseViewModel(T data, string message = "")
        {
            Data = data;
            IsSuccess = true;
            Message = message;
            ErrorCode = ErrorCode.None;
        }
    }

    public class FailureResponseViewModel<T> : ResponseViewModel<T>
    {
        public FailureResponseViewModel(ErrorCode errorCode, string message = "" )
        {
            Data = default;
            IsSuccess = false;
            Message = message;
            ErrorCode = errorCode;
        }
        public static FailureResponseViewModel<T> NotFound( string message = "")
        {
            return new FailureResponseViewModel<T>(ErrorCode.NotFound, message ?? ErrorCode.NotFound.ToString());
        }
        public static FailureResponseViewModel<T> UnAuthorized(string message = "")
        {
            return new FailureResponseViewModel<T>(ErrorCode.UnAuthorized, message ?? ErrorCode.UnAuthorized.ToString());
        }
        public static FailureResponseViewModel<T> BadRequest(string message = "")
        {
            return new FailureResponseViewModel<T>(ErrorCode.BadRequest, message ?? ErrorCode.BadRequest.ToString());
        }
    }
}
