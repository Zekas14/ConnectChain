using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.ShippingAddress.AddShippingAddress;
using MediatR;

namespace ConnectChain.Features.CustomerManagement.ShippingAddress.AddShippingAddress.Command
{
    public record AddShippingAddressCommand(
          string CustomerId,
          string Address,
          string Apartment,
          string City,
          string Region,
          string Phone) : IRequest<RequestResult<AddShippingAddressRequestViewModel>>;

    public class AddShippingAddressCommandHandler(IRepository<UserShippingAddress> repository)
        : IRequestHandler<AddShippingAddressCommand, RequestResult<AddShippingAddressRequestViewModel>>
    {
        private readonly IRepository<UserShippingAddress> repository = repository;

        public async Task<RequestResult<AddShippingAddressRequestViewModel>> Handle(AddShippingAddressCommand request, CancellationToken cancellationToken)
        {
            var shippingAddress = new UserShippingAddress
            {
                UserId = request.CustomerId,
                Address = request.Address,
                Apartment = request.Apartment,
                City = request.City,
                Region = request.Region,
                Phone = request.Phone,
                CreatedDate = DateTime.UtcNow
            };

            await repository.AddAsync(shippingAddress);

            var response = new AddShippingAddressRequestViewModel
            {
                Address = shippingAddress.Address,
                Apartment = shippingAddress.Apartment,
                City = shippingAddress.City,
                Region = shippingAddress.Region,
                Phone = shippingAddress.Phone
            };

            return RequestResult<AddShippingAddressRequestViewModel>.Success(response, "Shipping address added successfully");
        }
    }
}
