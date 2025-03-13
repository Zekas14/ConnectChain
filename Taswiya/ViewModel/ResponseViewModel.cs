using Microsoft.AspNetCore.Http;
using ConnectChain.Helpers;

namespace ConnectChain.ViewModel
{
    public class ResponseViewModel<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public ErrorCode ErrorCode { get; set; }
        public string Message { get; set; }
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

    public class FaluireResponseViewModel<T> : ResponseViewModel<T>
    {
        public FaluireResponseViewModel(ErrorCode errorCode, string message = "" )
        {
            Data = default;
            IsSuccess = false;
            Message = message;
            ErrorCode = errorCode;
        }
        public static FaluireResponseViewModel<T> NotFound( string message = "")
        {
            return new FaluireResponseViewModel<T>(ErrorCode.NotFound, message ?? ErrorCode.NotFound.ToString());
        }
        public static FaluireResponseViewModel<T> UnAuthorized(string message = "")
        {
            return new FaluireResponseViewModel<T>(ErrorCode.UnAuthorized, message ?? ErrorCode.UnAuthorized.ToString());
        }
        public static FaluireResponseViewModel<T> BadRequest(string message = "")
        {
            return new FaluireResponseViewModel<T>(ErrorCode.BadRequest, message ?? ErrorCode.BadRequest.ToString());
        }
    }
}
