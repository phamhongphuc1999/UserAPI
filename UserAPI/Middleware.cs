// Copyright (c) Microsoft. All Rights Reserved.
// License under the Apache License, Version 2.0.
// API with mongodb, SQL server database and more.
// Owner: Pham Hong Phuc

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace UserAPI
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private string mainUrl;

        public LoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, string mainUrl)
        {
            this.mainUrl = mainUrl;
            _next = next;
            _logger = loggerFactory.CreateLogger<LoggerMiddleware>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await _next(httpContext);
            int statusCode = httpContext.Response.StatusCode;
            if (statusCode >= 200 && statusCode < 300) _logger.LogInformation("{0}: {1}{2} => {3}",
                    httpContext.Request.Method, mainUrl,
                    httpContext.Request.Path, statusCode);
            else _logger.LogError("{0}: {1}{2} => {3}",
                   httpContext.Request.Method, mainUrl,
                   httpContext.Request.Path, statusCode);
        }
    }
}
