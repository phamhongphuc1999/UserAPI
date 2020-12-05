using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace UserAPI
{
    public class Routes
    {
        private static RequestDelegate Middleware(RequestDelegate next)
        {
            return new RequestDelegate(async (context) =>
            {
                context.Request.Headers.Add("abc", "123");
                await next(context);
            });
        }

        public static IRouter BuildRouter(IApplicationBuilder applicationBuilder)
        {
            RouteBuilder builder = new RouteBuilder(applicationBuilder);
            builder.MapMiddlewareGet("/login", appBuilder => {
                appBuilder.Use(Middleware);
            });
            return builder.Build();
        }
    }
}
