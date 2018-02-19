using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Middleware;
using Newtonsoft.Json;

namespace TestApp.Web.Controllers
{
    [Route("api/[controller]")]
    public class LoggerController : Controller
    {
        private IMiddlewareLogger _logger;
        public LoggerController(IMiddlewareLogger logger)
        {
            this._logger = logger;
        }

        [HttpGet("[action]")]
        public IEnumerable<RequestLog> GetLogs()
        {
            return this._logger.GetLogs(); ;
        }
    }
}