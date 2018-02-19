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
    public interface IMiddlewareLogger
    {
        RequestLog HandleRequest(HttpRequest request, DateTime now);
        RequestLog HandleResponse(HttpResponse response, RequestLog requestLog);
        IEnumerable<RequestLog> GetLogs();
        RequestLog LogRequest(RequestLog requestLog);
    }
}