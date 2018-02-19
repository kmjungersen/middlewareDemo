using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyMiddleware.Objects
{
    public class RequestLog
    {
        // Status Code - e.g. 200, 404, etc.
        public int? StatusCode { get; set; }

        // Method - e.g. GET, POST, etc.
        public string Method { get; set; }

        // Path - the full path of the request inclduing the URL and query string
        public string Path { get; set; }

        // Size - the size of the request in bytes
        public string Size { get; set; }

        // Request Time - the timestamp of the request
        public DateTime RequestTime { get; set; }

        // Request Time - the timestamp of the response
        public DateTime ResponseTime { get; set; }

        // Total Request Time - the start to end time in milliseconds
        public string TotalRequestTime { get; set; }

        // Processing Time - the total time spent processing the logs in milliseconds
        public string ProcessingTime { get; set; }
    }
}