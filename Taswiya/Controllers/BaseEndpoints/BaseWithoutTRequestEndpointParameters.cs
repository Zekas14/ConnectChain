using MediatR;

namespace ConnectChain.Controllers.BaseEndpoints;
public class BaseWithoutTRequestEndpointParameters
{
    readonly IMediator _mediator;
    
    public IMediator Mediator => _mediator;

   
    public BaseWithoutTRequestEndpointParameters(IMediator mediator)
    {
        _mediator = mediator;
    }
}

