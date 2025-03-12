using FluentValidation;
using MediatR;

namespace ConnectChain.Controllers.BaseEndpoints;
public class BaseEndpointParameters<TRequest>
{
    readonly IMediator _mediator;
    readonly IValidator<TRequest> _validator;

    
    public IMediator Mediator => _mediator;
    public IValidator<TRequest> Validator => _validator;
   
    public BaseEndpointParameters(IMediator mediator, IValidator<TRequest> validator)
    {
        _mediator = mediator;
        _validator = validator;

    }
}

