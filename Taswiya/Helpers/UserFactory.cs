using ConnectChain.Models;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
                    BusinessType = viewModel.BusinessType,
                    FcmToken = viewModel.FcmToken,
                },
                Role.Customer => new Customer
                {
                    Name = viewModel.Name,
                    Email = viewModel.Email,
                    UserName = viewModel.Email,
                    PhoneNumber = viewModel.PhoneNumber,
                    Address = viewModel.Address,
                    BusinessType = viewModel.BusinessType,
                    FcmToken = viewModel.FcmToken,
                },
                _ => throw new ArgumentOutOfRangeException(nameof(viewModel.Role), "Unsupported Role")
            };
        }
        public static async Task<JwtSecurityToken> CreateTokenAsync(this UserManager<User> userManager, User user)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            List<Claim> claims =
                [
                    new Claim(ClaimTypes.Name, user.Name!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                ];
            var roles = await userManager.GetRolesAsync(user);
            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(config["JWT:SecretKey"]!));
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken
              (issuer: config["JWT:Issuer"],
               audience: config["JWT:Audience"],
               claims: claims,
               expires: DateTime.Now.AddHours(7),
               signingCredentials: signingCredentials);
            return token;
        }
        public static JwtSecurityToken GetToken(this HttpRequest request)
        {
            var authHeader = request.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer"))
            {
                return null!;
            }
            var token = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken? jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            return jsonToken ?? null;
        }
        public static string? GetIdFromToken(this HttpRequest request)
        {
            var jsonToken = request.GetToken();
            return jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? null;
        }
        public static Role? GetRoleFromToken(this HttpRequest request)
        {
            var jsonToken = request.GetToken();
            if (jsonToken is null || !UserFactory.IsTokenValid(jsonToken))
            {
                return null;
            }
            var role = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            return Enum.TryParse<Role>(role, out var parsedRole) ? parsedRole : null;
        }
        public static bool IsAuthenticated(this HttpRequest request)
        {
            var token = request.GetToken();
            if (token is null || !IsTokenValid(token))
            {
                return false;
            }
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
