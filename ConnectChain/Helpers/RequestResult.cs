namespace ConnectChain.Helpers
{
    public record RequestResult<T>(T data, bool isSuccess, string message, ErrorCode errorCode)
    {

        public static RequestResult<T> Success(T data, string message = "")
        {
            return new RequestResult<T>(data, true, message, ErrorCode.None);
        }

        public static RequestResult<T> Failure(ErrorCode errorCode)
        {
            return new RequestResult<T>(default!, false, errorCode.ToString(), errorCode);
        }

        public static RequestResult<T> Failure(ErrorCode errorCode, string message)
        {
            return new RequestResult<T>(default!, false, message, errorCode);
        }
    }
}
