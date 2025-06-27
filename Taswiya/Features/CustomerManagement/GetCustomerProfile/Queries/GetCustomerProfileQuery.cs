using ConnectChain.Data.Context;
using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Customer;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.CustomerManagement.GetCustomerProfile.Queries
{
    public record GetCustomerProfileQuery(string CustomerId) : IRequest<RequestResult<CustomerProfileViewModel>>;
    
    public class GetCustomerProfileQueryHandler(ConnectChainDbContext context) 
        : IRequestHandler<GetCustomerProfileQuery, RequestResult<CustomerProfileViewModel>>
    {
        private readonly ConnectChainDbContext _context = context;

        public async Task<RequestResult<CustomerProfileViewModel>> Handle(GetCustomerProfileQuery request, CancellationToken cancellationToken)
        {
            var customer = await _context.Set<Customer>().Where(c => c.Id == request.CustomerId)
                .Select(c => new CustomerProfileViewModel
                {
                    Id = c.Id,
                    Name = c.Name ?? string.Empty,
                    Email = c.Email ?? string.Empty,
                    PhoneNumber = c.PhoneNumber ?? string.Empty,
                    Address = c.Address ?? string.Empty,
                    BusinessType = c.BusinessType ?? string.Empty,
                    ImageUrl = c.ImageUrl ?? string.Empty
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (customer == null)
            {
                return RequestResult<CustomerProfileViewModel>.Failure(ErrorCode.NotFound, "Customer not found.");
            }

            return RequestResult<CustomerProfileViewModel>.Success(customer, "Customer profile retrieved successfully.");
        }
    }
}
