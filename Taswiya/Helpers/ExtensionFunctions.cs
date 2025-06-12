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
             public static string GetBaseUrl(this HttpRequest request)
        {
                return $"{request.Scheme}://{request.Host.Value}{request.PathBase.Value}";
        }
        }

}
