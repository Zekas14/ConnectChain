using ConnectChain.Data.Repositories.Repository;
using ConnectChain.Features.ImageManagement.UploadImage.Command;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectChain.Features.RFQManagement.CreateRFQ.commands
{
    public record CreateRFQCommand(
        string CustomerId,
        int? ProductId,
        string ProductName,
        int CategoryId,
        string? Description,
        int Quantity,
        string Unit,
        DateTime? Deadline,
        bool ShareBusinessCard,
        List<IFormFile>? Attachments
    ) : IRequest<RequestResult<int>>;

    public class CreateRFQCommandHandler : IRequestHandler<CreateRFQCommand, RequestResult<int>>
    {
        private readonly IRepository<RFQ> _rfqRepository;
        private readonly IRepository<RfqAttachment> _attachmentRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IMediator _mediator;
        private readonly CloudinaryService _cloudinaryService;

        public CreateRFQCommandHandler(
            IRepository<RFQ> rfqRepository,
            IRepository<RfqAttachment> attachmentRepository,
            IRepository<Product> productRepository,
            IRepository<Category> categoryRepository,
            IMediator mediator,
            CloudinaryService cloudinaryService)
        {
            _rfqRepository = rfqRepository;
            _attachmentRepository = attachmentRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mediator = mediator;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<RequestResult<int>> Handle(CreateRFQCommand request, CancellationToken cancellationToken)
        {

            var categoryExists = await _categoryRepository.AnyAsync(c => c.ID == request.CategoryId);
            if (!categoryExists)
            {
                return RequestResult<int>.Failure(ErrorCode.InvalidInput, $"Invalid CategoryId: {request.CategoryId}");
            }

            if (request.ProductId.HasValue)
            {
                var product = _productRepository.GetByID(request.ProductId.Value);
                if (product == null)
                {
                    return RequestResult<int>.Failure(ErrorCode.InvalidInput, $"Invalid ProductId: {request.ProductId.Value}");
                }

                if (product.CategoryId != request.CategoryId)
                {
                    return RequestResult<int>.Failure(ErrorCode.InvalidInput, $"ProductId {request.ProductId.Value} does not belong to CategoryId {request.CategoryId}.");
                }
            }


            var uploadedImageUrls = new List<string>();
            var uploadedPublicIds = new List<string>();
            var attachments = new List<RfqAttachment>();

            try
            {
                if (request.Attachments != null)
                {
                    foreach (var file in request.Attachments)
                    {
                        var uploadResult = await _mediator.Send(new UploadImageCommand(file), cancellationToken);
                        if (!uploadResult.isSuccess || string.IsNullOrEmpty(uploadResult.data))
                        {
                           
                            foreach (var pubId in uploadedPublicIds)
                            {
                                await _cloudinaryService.DeleteImageAsync(pubId);
                            }
                           
                            return RequestResult<int>.Failure(
                                ErrorCode.InternalServerError,
                                $"Failed to upload attachment: '{file.FileName}'. Error: {uploadResult.message}"
                            );
                        }

                        uploadedImageUrls.Add(uploadResult.data);

                        
                        var publicId = ExtractPublicIdFromUrl(uploadResult.data);
                        if (!string.IsNullOrEmpty(publicId))
                            uploadedPublicIds.Add(publicId);

                        attachments.Add(new RfqAttachment
                        {
                            FileUrl = uploadResult.data
                        });
                    }
                }

               
                var rfq = new RFQ
                {
                    CustomerId = request.CustomerId,
                    ProductId = request.ProductId,
                    ProductName = request.ProductName,
                    CategoryId = request.CategoryId,
                    Description = request.Description,
                    Quantity = request.Quantity,
                    Unit = request.Unit,
                    Deadline = request.Deadline,
                    ShareBusinessCard = request.ShareBusinessCard,
                    Status = Models.Enums.RfqStatus.Pending
                };

                _rfqRepository.Add(rfq);
                await _rfqRepository.SaveChangesAsync();

                
                if (attachments.Count > 0)
                {
                    foreach (var att in attachments)
                        att.RfqId = rfq.ID;

                    _attachmentRepository.AddRange(attachments);
                    await _attachmentRepository.SaveChangesAsync();
                }

                return RequestResult<int>.Success(rfq.ID, "RFQ created successfully");
            }
            catch (Exception ex)
            {
                
                foreach (var publicId in uploadedPublicIds)
                {
                    await _cloudinaryService.DeleteImageAsync(publicId);
                }
                return RequestResult<int>.Failure(ErrorCode.InternalServerError, $"Failed to create RFQ: {ex.Message}");
            }
        }
        
        private static string ExtractPublicIdFromUrl(string url)
        {
            var match = Regex.Match(url, @"/upload/(?:v\d+/)?(.+)\.[a-zA-Z0-9]+$");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }
    }
}