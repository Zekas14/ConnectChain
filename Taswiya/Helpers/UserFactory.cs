using ConnectChain.Models;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace ConnectChain.Helpers
{
    public static class UserFactory
    {
        public static User CreateUser(UserRegisterRequestViewModel viewModel)
        {
            return viewModel.Role switch
            {
                Role.Supplier => new Supplier
                {
                    Name= viewModel.Name,
                    Email = viewModel.Email,
                    UserName = viewModel.Email,
                    PhoneNumber = viewModel.PhoneNumber,
                    Address = viewModel.Address,
                    BusinessType = viewModel.BusinessType
                },
                Role.Customer => new Customer
                {
                    Name = viewModel.Name,
                    Email = viewModel.Email,
                    UserName = viewModel.Email,
                    PhoneNumber = viewModel.PhoneNumber,
                    Address = viewModel.Address,
                    BusinessType = viewModel.BusinessType
                },
                _ => throw new ArgumentOutOfRangeException(nameof(viewModel.Role), "Unsupported Role")
            };
        }
        public static bool IsAuthenticated(this HttpRequest request)
        {
            var token = request.GetToken();
            if (token is null || !IsTokenValid(token))
            {
                return false;
            }
            var role = request.GetRoleFromToken();
            return request.GetIdFromToken() != null && request.GetRoleFromToken() != null;

        }
        public static bool IsTokenValid(this JwtSecurityToken jsonToken)
        {
            var expirationDate = jsonToken!.ValidTo;
            return expirationDate > DateTime.UtcNow;
        }
        public static bool IsRoleAuthorzied(this HttpRequest request, params Role[] roles)
        {
            var userRole = request.GetRoleFromToken();
            return userRole.HasValue && roles.Contains(userRole.Value);
        }

    }

}
