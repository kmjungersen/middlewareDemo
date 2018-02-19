using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Middleware
{
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;
        private IMiddlewareLogger _logger;

        public MyMiddleware(RequestDelegate next, IMiddlewareLogger middlewareLogger)
        {
            this._next = next;
            this._logger = middlewareLogger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var requestLog = this._logger.HandleRequest(context.Request);
            stopwatch.Stop();

            await this._next.Invoke(context);

            stopwatch.Start();
            requestLog = this._logger.HandleResponse(context.Response, requestLog);
            stopwatch.Stop();

            requestLog.ProcessingTime = ((long)(stopwatch.ElapsedTicks / 10000)).ToString();

            this._logger.LogRequest(requestLog);
        }
    }

    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMiddleware>();
        }
    }

    public interface IMiddlewareLogger
    {
        RequestLog HandleRequest(HttpRequest request);
        RequestLog HandleResponse(HttpResponse response, RequestLog requestLog);
        IEnumerable<RequestLog> GetLogs();
        RequestLog LogRequest(RequestLog requestLog);
    }

    public class MiddlewareLogger : IMiddlewareLogger
    {
        private List<RequestLog> _fullCycleLogs;
        public MiddlewareLogger()
        {
            this._fullCycleLogs = new List<RequestLog>();
        }

        public RequestLog HandleRequest(HttpRequest request)
        {
            var requestUrl = request.Path + request.QueryString.ToString();

            var requestLog = new RequestLog()
            {
                Method = request.Method,
                Path = requestUrl,
                Size = request.ContentLength.ToString(),
                RequestTime = DateTime.Now,
            };

            return requestLog;
        }

        public RequestLog HandleResponse(HttpResponse response, RequestLog requestLog)
        {
            requestLog.ResponseTime = DateTime.Now;
            requestLog.StatusCode = response.StatusCode;
            requestLog.Size = response.ContentLength.ToString();
            var totalRequestTime = requestLog.ResponseTime.Subtract(requestLog.RequestTime);
            requestLog.TotalRequestTime = ((long)(totalRequestTime.Ticks / 10000)).ToString();

            return requestLog;
        }

        public IEnumerable<RequestLog> GetLogs()
        {
            return this._fullCycleLogs;
        }

        public RequestLog LogRequest(RequestLog requestLog)
        {
            this._fullCycleLogs.Add(requestLog);

            return requestLog;
        }
    }

    public class RequestLog
    {
        public int? StatusCode { get; set; }
        public string Method { get; set; }
        // public string Body { get; set; }
        public string Path { get; set; }
        public string Size { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime ResponseTime { get; set; }
        public string TotalRequestTime { get; set; }
        public string ProcessingTime { get; set; }
    }
}