using ConnectChain.Data.Context;
using ConnectChain.Helpers;
using ConnectChain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ConnectChain.Features.SupplierManagement.UpdateSupplierProfile.Commands
{
    public record UpdateSupplierProfileCommand : IRequest<RequestResult<bool>>
    {
        public string Id { get; init; }

        public string? Name { get; set; }

        public string? PhoneNumber { get; init; }

        public string? Address { get; init; }
        public int? ActivityCategoryID { get; init; }
        public List<int>? PaymentMethodsIDs { get; init; }
    }

    public class UpdateSupplierProfileCommandHandler : IRequestHandler<UpdateSupplierProfileCommand, RequestResult<bool>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ConnectChainDbContext _dbContext;

        public UpdateSupplierProfileCommandHandler(UserManager<User> userManager, ConnectChainDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<RequestResult<bool>> Handle(UpdateSupplierProfileCommand request, CancellationToken cancellationToken)
        {
            var supplier = _dbContext.Suppliers
                .Include(s => s.PaymentMethods)
                .FirstOrDefault(sup => sup.Id == request.Id);

            if (supplier == null)
            {
                return RequestResult<bool>.Failure(ErrorCode.NotFound, "Supplier not found.");
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
                supplier.Name = request.Name;

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                supplier.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(request.Address))
                supplier.Address = request.Address;


            if (request.ActivityCategoryID.HasValue)
            {
                var activityCategory = await _dbContext.ActivityCategories
                    .FirstOrDefaultAsync(ac => ac.ID == request.ActivityCategoryID.Value, cancellationToken);

                if (activityCategory == null)
                {
                    return RequestResult<bool>.Failure(ErrorCode.InvalidInput, "Invalid ActivityCategoryID.");
                }

                supplier.ActivityCategoryId = request.ActivityCategoryID.Value;
            }


            if (request.PaymentMethodsIDs != null && request.PaymentMethodsIDs.Any())
            {
                var existingPaymentMethodIds = supplier.PaymentMethods.Select(pm => pm.ID).ToList();
                var newPaymentMethodIds = request.PaymentMethodsIDs.Except(existingPaymentMethodIds).ToList();
                var removedPaymentMethodIds = existingPaymentMethodIds.Except(request.PaymentMethodsIDs).ToList();


                if (newPaymentMethodIds.Any())
                {
                    var newPaymentMethods = await _dbContext.PaymentMethods
                        .Where(pm => newPaymentMethodIds.Contains(pm.ID))
                        .ToListAsync(cancellationToken);

                    if (newPaymentMethods.Count != newPaymentMethodIds.Count)
                    {
                        return RequestResult<bool>.Failure(ErrorCode.InvalidInput, "One or more PaymentMethodsIDs are invalid.");
                    }

                    foreach (var paymentMethod in newPaymentMethods)
                    {
                        supplier.PaymentMethods.Add(paymentMethod);
                    }
                }


                if (removedPaymentMethodIds.Any())
                {
                    var removedPaymentMethods = supplier.PaymentMethods
                        .Where(pm => removedPaymentMethodIds.Contains(pm.ID))
                        .ToList();

                    foreach (var paymentMethod in removedPaymentMethods)
                    {
                        supplier.PaymentMethods.Remove(paymentMethod);
                    }
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return RequestResult<bool>.Success(true, "User profile updated successfully.");
        }
    }
}
