using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MyMiddleware.Objects;
using System.IO;

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

            // Store the original body stream of the response so we can put it back later
            var origBody = context.Response.Body;

            try {
                using (var memStream = new MemoryStream()){
                    // Set the response body to be our new memory stream so we can read it later
                    context.Response.Body = memStream;

                    // Log the initial request
                    var requestLog = this._logger.HandleRequest(context.Request, now);

                    stopwatch.Stop();

                    // After logging the request, go ahead an continue down the request pipeline to the next action
                    await this._next.Invoke(context);

                    // Now that we've gone to the "end" of the pipeline and the response is coming back, we're going
                    // to finish our logging.  First, resume the stopwatch.
                    stopwatch.Start();

                    // Start reading at position 0
                    memStream.Position = 0;

                    // Read the whole response stream so we can measure it
                    var responseBody = new StreamReader(memStream).ReadToEnd();

                    // Move back to the beginning so we can copy the content back to our original body
                    memStream.Position = 0;

                    // Copy the stream back to the original body stream
                    await memStream.CopyToAsync(origBody);

                    // Tada!  Now we can properly read the length of the response body
                    requestLog.Size = responseBody.Length.ToString();

                    // Handle the rest of the response
                    requestLog = this._logger.HandleResponse(context.Response, requestLog);

                    // Stop the stopwatch
                    stopwatch.Stop();
                    // Record the time delta in Milliseconds = (Ticks / 10,000), so we can get a more accurate decimal 
                    // without rounding
                    requestLog.ProcessingTime = ((double)(stopwatch.ElapsedTicks) / 10000.0).ToString();

                    // Now that we're done, log the full request/response and return to the pipeline
                    this._logger.LogRequest(requestLog);
                }
            } finally {
                // After all of that, replace the body with the "original" response body
                context.Response.Body = origBody;
            }
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