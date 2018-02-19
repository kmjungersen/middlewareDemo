using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MyMiddleware.Objects;

namespace MyMiddleware
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

        // Invoke() is called with the HttpContext in the request pipeline. Although this middleware can
        // be injected anywhere, I chose to inject this at the begining of the request / end of the response.
        public async Task Invoke(HttpContext context)
        {
            // Create new Stopwatch for measuring handling time
            var stopwatch = new Stopwatch();
            var now = DateTime.Now;

            // Start the stopwatch before doing anything
            stopwatch.Start();
            var requestLog = this._logger.HandleRequest(context.Request, now);
            stopwatch.Stop();

            // After logging the request, go ahead an continue down the request pipeline to the next action
            await this._next.Invoke(context);

            // Now that we've gone to the "end" of the pipeline and the response is coming back, we're going
            // to finish our logging.  First, resume the stopwatch.
            stopwatch.Start();
            requestLog = this._logger.HandleResponse(context.Response, requestLog);
            stopwatch.Stop();

            // Calculate the time by converting ticks to milliseconds to we retain the decimal value
            requestLog.ProcessingTime = (((long)stopwatch.ElapsedTicks / 10000)).ToString();

            // Now that we're done, log the full request/response and return to the pipeline
            this._logger.LogRequest(requestLog);
        }
    }

    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            // Register our middleware with the IApplicationBuilder so it can be invoked in Startup.cs
            return builder.UseMiddleware<MyMiddleware>();
        }
    }
}