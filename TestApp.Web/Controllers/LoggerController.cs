using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using MyMiddleware;
using MyMiddleware.Objects;
using TestApp.Web.Objects;

namespace TestApp.Web.Controllers
{
    [Route("api/[controller]")]
    public class LoggerController : Controller
    {
        private IMiddlewareLogger _logger;

        // Inject the IMiddlewareLogger from the Startup.cs Service registration
        public LoggerController(IMiddlewareLogger logger)
        {
            this._logger = logger;
        }

        [HttpGet("[action]")]
        public IEnumerable<RequestLog> GetLogs()
        {
            // Return a list of logs
            return this._logger.GetLogs();
        }

        [HttpGet("[action]")]
        public ResultStatistics GetStatistics()
        {
            var logs = this._logger.GetLogs();

            var resultStats = new ResultStatistics
            {
                MaxRequest = logs.Max(x => Convert.ToInt32(x.Size)).ToString(),
                MinRequest = logs.Min(x => Convert.ToInt32(x.Size)).ToString(),
                AvgRequest = logs.Average(x => Convert.ToInt32(x.Size)).ToString(),
            };

            return resultStats;
        }
    }
}