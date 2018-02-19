using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MyMiddleware.Objects;

namespace MyMiddleware.Objects
{
    public class MiddlewareLogger : IMiddlewareLogger
    {
        private List<RequestLog> _fullCycleLogs;
        public MiddlewareLogger()
        {
            this._fullCycleLogs = new List<RequestLog>();
        }

        // This method is called with the HttpRequest Object at the very BEGINNING of every request in the pipeline
        public RequestLog HandleRequest(HttpRequest request, DateTime now)
        {
            // Add the base path and the query string for the full URL
            var requestUrl = request.Path + request.QueryString.ToString();

            // Create a new Log object with the method, full path, and start time
            var requestLog = new RequestLog()
            {
                Method = request.Method,
                Path = requestUrl,
                RequestTime = now,
            };

            // Return the log object
            return requestLog;
        }

        // This method is called with the HttpResponse object at the very END of every request in the pipeline
        public RequestLog HandleResponse(HttpResponse response, RequestLog requestLog)
        {
            // Set the status code and size of response
            requestLog.StatusCode = response.StatusCode;
            requestLog.Size = response.ContentLength.ToString();

            // Set the Response time and then calculate the total request time
            requestLog.ResponseTime = DateTime.Now;
            var totalRequestTime = requestLog.ResponseTime.Subtract(requestLog.RequestTime);

            // Since the entire request can be a fraction of a milisecond in this app, I'm measuring in ticks (1/10,000 of a millisecond)
            // and then converting to milliseconds to retain the decimal instead of using foo.ToMilliseconds, which would round the number
            requestLog.TotalRequestTime = ((long)(totalRequestTime.Ticks / 10000)).ToString();

            // Return the updated request log
            return requestLog;
        }

        public IEnumerable<RequestLog> GetLogs()
        {
            // Return all of the logs so far, which ironically won't yet include the current request to get all logs.  Originally I logged
            // requests and responses seperately, in which case it would properly return the current request without the response, but I
            // felt it was more useful to look at the full request lifecycle rather than both "halves" being split by the rest of the request
            // pipeline in .NET.
            return this._fullCycleLogs;
        }

        public RequestLog LogRequest(RequestLog requestLog)
        {
            // Add a new entry to the logs
            this._fullCycleLogs.Add(requestLog);

            return requestLog;
        }
    }
}