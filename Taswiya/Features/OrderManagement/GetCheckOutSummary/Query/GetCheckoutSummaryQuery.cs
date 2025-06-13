using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.CartManagement.Cart.Queries.GetCart.Query;
using ConnectChain.Features.CustomerManagement.ShippingAddress.GetShippingAddresses.Query;
using ConnectChain.Features.ProductManagement.Common.Queries;
using ConnectChain.Helpers;
using ConnectChain.ViewModel.Order.GetCheckOutSummary;
using MediatR;

namespace ConnectChain.Features.OrderManagement.GetCheckOutSummary.Query
{
    public record GetCheckoutSummaryQuery(string CustomerId) : IRequest<RequestResult<CheckoutSummaryResponseViewModel>>;
    public class GetCheckoutSummaryQueryHandler(IMediator mediator)
       : IRequestHandler<GetCheckoutSummaryQuery, RequestResult<CheckoutSummaryResponseViewModel>>
    {
        private readonly IMediator _mediator = mediator;

        public async Task<RequestResult<CheckoutSummaryResponseViewModel>> Handle(GetCheckoutSummaryQuery request, CancellationToken cancellationToken)
        {
            var cartResult = await _mediator.Send(new GetCartQuery(request.CustomerId), cancellationToken);
            if (!cartResult.isSuccess || cartResult.data == null || !cartResult.data.Items.Any())
                return RequestResult<CheckoutSummaryResponseViewModel>.Failure(ErrorCode.NotFound, "Cart is empty");

            var cart = cartResult.data;

            // 2. Get Product Data
            var productIds = cart.Items.Select(i => i.ProductId).ToList();
            var productsResult = await _mediator.Send(new GetExistingProductsQuery(productIds), cancellationToken);
            if (!productsResult.isSuccess)
                return RequestResult<CheckoutSummaryResponseViewModel>.Failure(productsResult.errorCode, productsResult.message);

            var products = productsResult.data;

            // 3. Map Cart Items
            var cartItems = cart.Items.Select(ci =>
            {
                var product = products.First(p => p.ID == ci.ProductId);
                return new CartItemViewModel
                {
                    ProductName = product.Name,
                    ImageUrl = product.Images.Select(i=>i.Url).FirstOrDefault(),
                    Price = product.Price,
                    Quantity = ci.Quantity
                };
            }).ToList();

            var subTotal = cartItems.Sum(i => i.Total);

            var shippingResult = await _mediator.Send(new GetShippingAddressesQuery(request.CustomerId), cancellationToken);
            var shippingAddresses = shippingResult.isSuccess ? shippingResult.data : [];

            var summary = new CheckoutSummaryResponseViewModel
            {
                Items = cartItems,
                SubTotal = subTotal,
                ShippingFees = 0,
                ShippingAddress = shippingAddresses.ToList()
            };

            return RequestResult<CheckoutSummaryResponseViewModel>.Success(summary, "Checkout summary fetched successfully.");
        }
    }

}
