using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UserAPI
{
    public class AuthorizedMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthorizedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path == "/users")
            {
                string token = httpContext.Request.Headers["token"];
                if (token == "null") await httpContext.Response.WriteAsync("You need login to active");
                else await _next.Invoke(httpContext);
            }
            else await _next.Invoke(httpContext);
        }
    }
}
