using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Net;

namespace UserAPI.Configuration
{
  [AttributeUsage(AttributeTargets.Method)]
  public class CustomAuthorization : Attribute, IAuthorizationFilter
  {
    public void OnAuthorization(AuthorizationFilterContext filterContext)
    {
      if (filterContext != null)
      {
        StringValues token;
        filterContext.HttpContext.Request.Headers.TryGetValue("token", out token);
        string _token = token.FirstOrDefault();
        if (_token != null)
        {
          string authToken = _token;
          if (Utilities.IsValidToken(authToken))
          {
            filterContext.HttpContext.Response.Headers.Add("authToken", authToken);
            filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");
            filterContext.HttpContext.Response.Headers.Add("storeAccessibility", "Authorized");
            return;
          }
          else
          {
            filterContext.HttpContext.Response.Headers.Add("authToken", authToken);
            filterContext.HttpContext.Response.Headers.Add("AuthStatus", "NotAuthorized");
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";
            filterContext.Result = new JsonResult("NotAuthorized")
            {
              Value = new
              {
                Status = "Error",
                Message = Messages.InvalidToken
              },
            };
          }
        }
        else
        {
          filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
          filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide authToken";
          filterContext.Result = new JsonResult("Please Provide authToken")
          {
            Value = new
            {
              Status = "Error",
              Message = Messages.Unauthorized
            },
          };
        }
      }
    }
  }
}
