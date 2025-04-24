using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ConnectChain.Models;
using ConnectChain.Models.Enums;

namespace ConnectChain.Helpers
{
    public static class ExtensionFunctions
    {
        public static  string GenerateCallBackUrl(this HttpRequest request, string userId)
        {
            var encodedUserId = Uri.EscapeDataString(userId);
            var callBackUrl = $"{request.Scheme}://{request.Host}/api/Account/ConfirmEmail?userId={encodedUserId}";
            return callBackUrl;
        }
        public static Role? GetRoleFromToken(this HttpRequest request) 
        {
            var jsonToken = request.GetToken();
            if (jsonToken is null || !UserFactory.IsTokenValid(jsonToken))
            {
                return null;
            }
            var role  = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            return Enum.TryParse<Role>(role, out var parsedRole) ? parsedRole : null;
        }
        public static string? GetIdFromToken(this HttpRequest request)
        {
            var jsonToken = request.GetToken();
            return jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? null;
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
            JwtSecurityToken? jsonToken = handler.ReadToken(token) as JwtSecurityToken ;
            return jsonToken?? null;
        }
        public static string GetBaseUrl(this HttpRequest request)
        {
                return $"{request.Scheme}://{request.Host.Value}{request.PathBase.Value}";
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
               expires: DateTime.Now.AddHours(1),
               signingCredentials: signingCredentials);
            return token;
            }
        }

}
