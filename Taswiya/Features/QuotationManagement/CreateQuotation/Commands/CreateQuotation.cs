using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.NotificationManagement.AddCustomerNotification.Commands;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ConnectChain.Features.QuotationManagement.CreateQuotation.Commands
{
    public record CreateQuotationCommand(
       int RfqId,
       string SupplierId,
       int ProductId,
       int Quantity,
       decimal UnitPrice,
       int PaymentTermId,
       double DeliveryFee,
       string DeliveryTerm,
       int DeliveryTimeInDays,
       string? Notes,
       DateTime ValidUntil
   ) : IRequest<RequestResult<int>>;


    public class CreateQuotationCommandHandler : IRequestHandler<CreateQuotationCommand, RequestResult<int>>
    {
        private readonly IRepository<Quotation> _quotationRepository;
        private readonly IRepository<RFQ> _rfqRepository;
        private readonly IMediator _mediator;
        private readonly IRepository<Product> _productRepository;

        public CreateQuotationCommandHandler(
            IRepository<Quotation> quotationRepository,
            IRepository<RFQ> rfqRepository,
            IRepository<Product> productRepository,
                    IMediator mediator)
        {
            _quotationRepository = quotationRepository;
            _rfqRepository = rfqRepository;
            _productRepository = productRepository;

            _mediator = mediator;
        }

        public async Task<RequestResult<int>> Handle(CreateQuotationCommand request, CancellationToken cancellationToken)
        {
           var rfq = _rfqRepository.GetAllWithIncludes(q => q
    .Where(x => x.ID == request.RfqId)
    .Include(x => x.SupplierAssignments)
    .Include(x => x.Product)
).FirstOrDefault();
            if (rfq == null)
                return RequestResult<int>.Failure(ErrorCode.NotFound, "RFQ not found.");

            var isAssigned = rfq.SupplierAssignments.Any(sa => sa.SupplierId == request.SupplierId);
            if (!isAssigned)
                return RequestResult<int>.Failure(ErrorCode.Forbidden, "Supplier is not assigned to this RFQ.");


            var existingQuotation = _quotationRepository.Get(q => q.RfqId == request.RfqId && q.SupplierId == request.SupplierId).FirstOrDefault();
            if (existingQuotation != null)
                return RequestResult<int>.Failure(ErrorCode.InvalidInput, "You have already submitted a quotation for this RFQ.");

            var product = _productRepository.GetByID(request.ProductId);
            if (product == null)
                return RequestResult<int>.Failure(ErrorCode.NotFound, "Product not found.");


            var categoryId = product.CategoryId;
            var quotation = new Quotation
            {
                RfqId = request.RfqId,
                SupplierId = request.SupplierId,
                ProductId = request.ProductId,
                CategoryId = categoryId,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice,
                PaymentTermId = request.PaymentTermId,
                DeliveryFee = request.DeliveryFee,
                DeliveryTerm = request.DeliveryTerm,
                DeliveryTimeInDays = request.DeliveryTimeInDays,
                Notes = request.Notes,
                ValidUntil = request.ValidUntil,
                Status = Models.Enums.QuotationStatus.Pending
            };

            _quotationRepository.Add(quotation);
            await _quotationRepository.SaveChangesAsync();
            var notificationCommand = new AddCustomerNotificationCommand(
           Title: "New Quotation Submitted",
           Body: $"A new quotation has been submitted for your RFQ #{rfq.ID}.",
           Type: "Quotation",
           CustomerId: rfq.CustomerId 
       );
            
           await _mediator.Send(notificationCommand, cancellationToken);

            return RequestResult<int>.Success(quotation.ID, "Quotation created successfully.");
        }
    }



}
