using CloudinaryDotNet.Actions;
using ConnectChain.Helpers;
using ConnectChain.Models.Enums;
using ConnectChain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace ConnectChain.Filters
{
    public class AuthorizationAttribute: Attribute , IAuthorizationFilter
    {
        private readonly Models.Enums.Role[] _roles  ;
        public AuthorizationAttribute(params Models.Enums.Role[] roles)
        {
            _roles = roles ?? throw new ArgumentNullException(nameof(roles));
            if (_roles.Length == 0)
                throw new ArgumentException("At least one role must be specified.");
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
            var request = context.HttpContext.Request;
            if (!request.IsAuthenticated())
            {
                context.Result =new FailureResponseViewModel<bool>(ErrorCode.UnAuthorized, "Invalid or missing token.");
                return;
            }
            if (!request.IsRoleAuthorzied(_roles))
            {
                context.Result = new FailureResponseViewModel<bool>(ErrorCode.Forbidden,"Role UnAuthorzied");
                return;
            }

        }
    }
}
