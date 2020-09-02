using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserAPI.JWT;

namespace UserAPI
{
    public class AuthorizedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private string secretKey;
        private string mainUrl;

        public AuthorizedMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, string secretKey, string mainUrl)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<AuthorizedMiddleware>();
            this.secretKey = secretKey;
            this.mainUrl = mainUrl;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            PathString path = httpContext.Request.Path;
            if (path.Value != "/login" && path.Value != "/logout")
            {
                string token = httpContext.Request.Headers["token"];
                if (token == "null")
                {
                    await httpContext.Response.WriteAsync("You Need To Login Before Action");
                    this.LoggerHandler(httpContext);
                }
                else
                {
                    IAuthService authService = new JWTService(secretKey);
                    List<Claim> claims = authService.GetTokenClaims(token).ToList();
                    string role = claims.Find(x => x.Type == "Role").Value;
                    if (role == "10") role = "admin";
                    else if (role == "01") role = "user";
                    string username = claims.Find(x => x.Type == ClaimTypes.Name).Value;
                    string password = claims.Find(x => x.Type == "Password").Value;
                    httpContext.Request.Headers.Add("username", username);
                    httpContext.Request.Headers.Add("password", password);
                    httpContext.Request.Headers.Add("role", role);
                    await _next(httpContext);
                    this.LoggerHandler(httpContext);
                }
            }
            else
            {
                await _next(httpContext);
                this.LoggerHandler(httpContext);
            }
        }

        private void LoggerHandler(HttpContext httpContext)
        {
            int statusCode = httpContext.Response.StatusCode;
            if (statusCode == 200) _logger.LogInformation("{0}: {1}{2} => {3}",
                    httpContext.Request.Method,
                    mainUrl,
                    httpContext.Request.Path,
                    statusCode);
            else _logger.LogError("{0}: {1}{2} => {3}",
                   httpContext.Request.Method,
                   mainUrl,
                   httpContext.Request.Path,
                   statusCode);
        }
    }
}
