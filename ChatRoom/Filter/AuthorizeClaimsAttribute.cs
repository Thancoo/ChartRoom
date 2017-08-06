using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using EasyNetQ.ConnectionString;
using Microsoft.AspNet.SignalR;

namespace ChatRoom.Filter
{
    public interface IAuthorizeClaimsAttribute
    {
        bool UserAuthorized(IPrincipal user);
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AuthorizeClaimsAttribute: AuthorizeAttribute
    {
        protected override bool UserAuthorized(System.Security.Principal.IPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var principal = user as ClaimsPrincipal;
            if (principal != null)
            {
                Claim authenticated = principal.FindFirst(ClaimTypes.Authentication);
                if (authenticated != null && authenticated.Value == "true")
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}