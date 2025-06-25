using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.CustomerManagement.Common.Queries
{
    public record IsCustomerExistsQuery(string CustomerId) : IRequest<RequestResult<bool>>;
    
    public class IsCustomerExistsQueryHandler(IRepository<Customer> repository) 
        : IRequestHandler<IsCustomerExistsQuery, RequestResult<bool>>
    {
        private readonly IRepository<Customer> _repository = repository;

        public async Task<RequestResult<bool>> Handle(IsCustomerExistsQuery request, CancellationToken cancellationToken)
        {
            var customerExists = await _repository.Get(c => c.Id == request.CustomerId)
                .AnyAsync(cancellationToken);

            if (!customerExists)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Customer not found.");
            }

            return RequestResult<bool>.Success(true, "Customer exists.");
        }
    }
}
