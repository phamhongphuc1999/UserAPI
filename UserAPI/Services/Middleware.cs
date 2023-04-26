using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserAPI.Services
{
  public class LoggerMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private string baseUrl;

    public LoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, string baseUrl)
    {
      this.baseUrl = baseUrl;
      _next = next;
      _logger = loggerFactory.CreateLogger<LoggerMiddleware>();
    }

    public async Task Invoke(HttpContext httpContext)
    {
      await _next(httpContext);
      int statusCode = httpContext.Response.StatusCode;
      string query = "";
      foreach (KeyValuePair<string, StringValues> item in httpContext.Request.Query)
      {
        query += string.Format("{0}={1}&", item.Key, item.Value);
      }
      string logString = "";
      if (query.Length > 0)
      {
        query = query.Substring(0, query.Length - 1);
        logString = string.Format("{0}: {1}{2}?{3} => {4}", httpContext.Request.Method, baseUrl, httpContext.Request.Path, query, statusCode);
      }
      else logString = string.Format("{0}: {1}{2} => {4}", httpContext.Request.Method, baseUrl, httpContext.Request.Path, statusCode);
      if (statusCode >= 200 && statusCode < 300)
        _logger.LogInformation(logString);
      else _logger.LogError(logString);
    }
  }
}
