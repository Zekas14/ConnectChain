
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ConnectChain.Helpers;  

namespace ConnectChain.Controllers.BaseEndpoints;

[ApiController]
[Route("[controller]/[action]")]

// [TypeFilter(typeof(UserInfoFilter))]
public class BaseEndpoint<TRequest, TResponse> : ControllerBase
{
    protected IMediator _mediator;
    protected IValidator<TRequest> _validator;

    public BaseEndpoint(BaseEndpointParameters<TRequest> parameters)
    {
        _mediator = parameters.Mediator;
        _validator = parameters.Validator;
    }

    protected EndpointResponse<TResponse> ValidateRequest(TRequest request)
    {
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var validationError = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));

            return EndpointResponse<TResponse>.Failure(ErrorCode.InvalidInput, validationError);
        }

        return EndpointResponse<TResponse>.Success(default);
    }
}
