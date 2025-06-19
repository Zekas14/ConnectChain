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

    public class UpdateCustomerProfileCommandHandler(IRepository<Customer> repository) 
        : IRequestHandler<UpdateCustomerProfileCommand, RequestResult<bool>>
    {
        private readonly IRepository<Customer> _repository = repository;

        public async Task<RequestResult<bool>> Handle(UpdateCustomerProfileCommand request, CancellationToken cancellationToken)
        {
            var customer = await _repository.Get(c => c.Id == request.CustomerId)
                .FirstOrDefaultAsync(cancellationToken);

            if (customer == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Customer not found.");
            }

            // Update only the fields that are provided (not null or empty)
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

            _repository.Update(customer);
            await _repository.SaveChangesAsync();

            return RequestResult<bool>.Success(true, "Customer profile updated successfully.");
        }
    }
}
