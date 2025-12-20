using ConnectChain.Data.Context;
using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.CustomerManagement.UpdateCustomerProfile.Commands
{
    public record UpdateCustomerProfileCommand : IRequest<RequestResult<bool>>
    {
        public string CustomerId { get; init; } = string.Empty;
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? BusinessType { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class UpdateCustomerProfileCommandHandler(ConnectChainDbContext context) 
        : IRequestHandler<UpdateCustomerProfileCommand, RequestResult<bool>>
    {
        private readonly ConnectChainDbContext context = context;

        public async Task<RequestResult<bool>> Handle(UpdateCustomerProfileCommand request, CancellationToken cancellationToken)
        {
            var customer = await context.Set<Customer>().AsTracking().FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken);

            if (customer == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Customer not found.");
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
                customer.Name = request.Name;

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                customer.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(request.Address))
                customer.Address = request.Address;

            if (!string.IsNullOrWhiteSpace(request.BusinessType))
                customer.BusinessType = request.BusinessType;

            if (!string.IsNullOrWhiteSpace(request.ImageUrl))
                customer.ImageUrl = request.ImageUrl;

            context.Update(customer);
            await context.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Customer profile updated successfully.");
        }
    }
}
