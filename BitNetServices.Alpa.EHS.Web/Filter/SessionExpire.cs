using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using BitNetServices.Alpa.EHS.Common.Model.Catalog;
using System;
using System.Linq;

namespace BitNetServices.Alpa.EHS.Web.Filter
{
    public class SessionExpire : ActionFilterAttribute
    {


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var _httpContextAccessor = filterContext.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
            if (_httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<UserModel>>("SessionUsers") == null)
            {
                filterContext.Result =
                   new RedirectToRouteResult(new RouteValueDictionary
                     {
                            { "action", "Index" },
                            { "controller", "Home" },
                            { "reason", "timeout"}
                   });

                return;
            }
        }
    }
    public class AuthorizedTenantOnly : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var _httpContextAccessor = filterContext.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
            if (_httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<UserModel>>("SessionUsers") != null)
            {
                UserModel user = null;
                var users = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<UserModel>>("SessionUsers");
                var urlPath = _httpContextAccessor.HttpContext.Request.Path.Value;
                string[] urlPieces = urlPath.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                if (urlPieces.Length > 0)
                {
                    var tenantName = urlPieces[0];

                    var usersList = users.Where(a => a.TenantUrlPiece == tenantName);
                    if (usersList != null && usersList.Count() > 0)
                    {
                        user = usersList.Last();
                    }
                }
                if (user == null)
                {
                    filterContext.Result =
                           new RedirectToRouteResult(new RouteValueDictionary
                             {
                                    { "action", "Unauthorise" },
                                    { "controller", "Account" }
                           });
                }
                return;
            }
        }
    }



}
