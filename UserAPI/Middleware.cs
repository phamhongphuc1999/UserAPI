using Microsoft.AspNetCore.Http;
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
        private string secretKey;

        public AuthorizedMiddleware(RequestDelegate next, string secretKey)
        {
            _next = next;
            this.secretKey = secretKey;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path == "/users")
            {
                string token = httpContext.Request.Headers["token"];
                if (token == "null") await httpContext.Response.WriteAsync("You Need To Login Before Action");
                else
                {
                    IAuthService authService = new JWTService(secretKey);
                    List<Claim> claims = authService.GetTokenClaims(token).ToList();
                    string role = claims.Find(x => x.Type == "Role").Value;
                    if (role == "10") role = "admin";
                    else if (role == "01") role = "user";
                    string username = claims.Find(x => x.Type == ClaimTypes.Name).Value;
                    string password = claims.Find(x => x.Type == "Password").Value;
                    //httpContext.Request.Headers.Add("username", username);
                    //httpContext.Request.Headers.Add("password", password);
                    //httpContext.Request.Headers.Add("role", role);
                    //httpContext.Items["password"] = password;
                    //httpContext.Items["username"] = username;
                    httpContext.Request.Headers.Add("Role", "123");

                    await _next(httpContext);
                }
            }
            else
            {
                httpContext.Request.Headers.Add("Role", "123");
                await _next(httpContext);
            }
        }
    }
}
