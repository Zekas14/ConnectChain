using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.ViewModel.Order.GetCheckOutSummary;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace ConnectChain.Features.CustomerManagement.ShippingAddress.GetShippingAddresses.Query
{
    public record GetShippingAddressesQuery(string CustomerId) : IRequest<RequestResult<IReadOnlyList<GetShippingAddressesResponseViewModel>>>;
    public class GetShippingAddressesQueryHandler(IRepository<UserShippingAddress> repository) : IRequestHandler<GetShippingAddressesQuery, RequestResult<IReadOnlyList<GetShippingAddressesResponseViewModel>>>
    {
        private readonly IRepository<UserShippingAddress> repository = repository;

        public async Task<RequestResult<IReadOnlyList<GetShippingAddressesResponseViewModel>>> Handle(GetShippingAddressesQuery request, CancellationToken cancellationToken)
        {
            var data = repository.Get(s => s.UserId == request.CustomerId).Select(s => new GetShippingAddressesResponseViewModel
            {
                Address = s.Address,
                Apartment = s.Apartment,
                City = s.City,
                Phone = s.Phone,
                Region = s.Region,
                
            });
            if (data.IsNullOrEmpty())
            {
                return RequestResult<IReadOnlyList<GetShippingAddressesResponseViewModel>>.Failure(ErrorCode.NotFound,"No Shipping Addresses");
            }
            return RequestResult<IReadOnlyList<GetShippingAddressesResponseViewModel>>.Success(data.ToList(), "Success");

        }
    }
}
