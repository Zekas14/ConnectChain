namespace ConnectChain.Helpers
{
    public enum ErrorCode
    {
        None=200,
        InvalidInput = 2,
        BadRequest = 400,
        UnAuthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 405,
        EmailAlreadyConfirmed = 409,
        InternalError = 410,
    }
}
