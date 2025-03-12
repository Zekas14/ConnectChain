using System.Security.Claims;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers.BaseEndpoints;

[ApiController]
[Route("[controller]/[action]")]

// [TypeFilter(typeof(UserInfoFilter))]
public class BaseWithoutTRequestEndpoint<TResponse> : ControllerBase
{
    protected IMediator _mediator;

    public BaseWithoutTRequestEndpoint(BaseWithoutTRequestEndpointParameters parameters)
    {
        _mediator = parameters.Mediator;
    }

}
