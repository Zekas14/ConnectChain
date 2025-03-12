namespace ConnectChain.Helpers
{
    public record EndpointResponse<T>(T Data, bool IsSuccess, object Message, ErrorCode ErrorCode)
    {
        public static EndpointResponse<T> Success(T data, string message = "")
        {
            return new EndpointResponse<T>(data, true, message, ErrorCode.None);
        }

        public static EndpointResponse<T> Failure(ErrorCode errorCode)
        {
            return new EndpointResponse<T>(default, false,errorCode.ToString(), errorCode);
        }

        public static EndpointResponse<T> Failure(ErrorCode errorCode, object message)
        {
            return new EndpointResponse<T>(default, false, message, errorCode);
        }
    }
}
