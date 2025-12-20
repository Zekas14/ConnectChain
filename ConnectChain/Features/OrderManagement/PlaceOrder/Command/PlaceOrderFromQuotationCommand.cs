using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.OrderManagement.PlaceOrder.Events;
using ConnectChain.Features.QuotationManagement.GetQuotation;
using ConnectChain.Helpers;
using ConnectChain.Models;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel.Quotation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.OrderManagement.PlaceOrder.Command
{
    public record PlaceOrderFromQuotationCommand(int QuotationId) : IRequest<RequestResult<bool>>;
    public class PlaceOrderFromQuotationCommandHandler
        (IRepository<Order> orderRepository, IMediator mediator, IRepository<Quotation> quotationRepository, IMailServices mailServices)
        : IRequestHandler<PlaceOrderFromQuotationCommand, RequestResult<bool>>
    {
        private readonly IRepository<Order> _orderRepository = orderRepository;
        private readonly IMediator _mediator = mediator;
        private readonly IRepository<Quotation> _quotationRepository = quotationRepository;
        private readonly IMailServices mailServices = mailServices;

        public async Task<RequestResult<bool>> Handle(PlaceOrderFromQuotationCommand request, CancellationToken cancellationToken)
        {

            var quotation = _quotationRepository.GetAllWithIncludes(q => q
                .Where(x => x.ID == request.QuotationId)
                .Include(x => x.Supplier)
                .Include(x => x.Product)
                .Include(x=>x.RFQ)
                .ThenInclude(x=>x.Customer)
            ).
            Select(x=>new
            {
                x.SupplierId,
                x.Product,
                x.RFQ.CustomerId,
                x.RFQ.Customer.Email,
                x.Quantity,
                x.UnitPrice,
               PaymentMethodName = x.PaymentTerm.Name,
                x.DeliveryFee,
                x.DeliveryTerm,
                x.Status
            })
            .FirstOrDefault();

            if (quotation == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Quotation not found.");
            }
            if (quotation.Status == QuotationStatus.Ordered)
            {
                return RequestResult<bool>.Failure(ErrorCode.InvalidInput, "Quotation Already Ordered.");

            }
            if (quotation.Product.Stock < quotation.Quantity)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Insufficient Quantity");
            }
            List<OrderItem> items =
            [
                new OrderItem()
                {
                 ProductId = quotation.Product.ID,
                 Quantity = quotation.Quantity,
                },
            ];
            
            Order order = new()
            {
                OrderItems = items,
                OrderNumber =Guid.NewGuid(),
                SupplierId =  quotation.SupplierId,
                CustomerId = quotation.CustomerId,
                SubTotal = (quotation.Quantity*quotation.UnitPrice),
                DeliveryFees = (decimal)(quotation.DeliveryFee),
                PaymentMethod = quotation.PaymentMethodName ,
                Notes = quotation.DeliveryTerm
                
                
            };
            _orderRepository.Add(order);
            _quotationRepository.SaveInclude(new Quotation() { ID = request.QuotationId,Status= QuotationStatus.Ordered},"Status");
             await _mediator.Publish(new OrderPlacedEvent(order, [quotation.Product]),cancellationToken);
            return RequestResult<bool>.Success(true,"Order Placed Successfully");
        }
    }
}
