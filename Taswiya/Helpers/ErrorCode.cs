namespace ConnectChain.Helpers
{
    public enum ErrorCode
    {
        None=200,
        InvalidInput = 2,
        BadRequest = 400,
        NotFound = 404,
        UnAuthorized = 401,
        InternalServerError = 405,
        EmailAlreadyConfirmed = 409,
        InternalError = 410,
    }
}
