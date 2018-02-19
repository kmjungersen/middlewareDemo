using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TestApp.Web.Objects;

namespace TestApp.Web.Controllers
{
    [Route("api/[controller]")]
    public class TestRouteController : Controller
    {
        public static string[] _baseRoutes = new[]
        {
            "Vader", "Yoda", "Windu", "Rey", "Skywalker", "Kenobi", "Grievous", "Leia", "Solo"
        };

        private IEnumerable<Route> _routes;

        public TestRouteController()
        {
            // Create list of Routes based on the base array of strings
            this._routes = Enumerable.Range(0, _baseRoutes.Length).Select(x => new Route
            {
                Name = _baseRoutes[x],
                Points = 0,
            });
        }

        [HttpGet("[action]")]
        public IEnumerable<Route> GetRoutes()
        {
            return this._routes;
        }

        [HttpGet("[action]")]
        public Route GetRoute(string routeName)
        {
            var route = this._routes.FirstOrDefault(x => x.Name == routeName);

            return route;
        }

        [HttpPost("[action]")]
        public Route PostRoute(Route postedRoute)
        {
            // Find the existing Route
            var existingRoute = this._routes.FirstOrDefault(x =>
                x.Name == postedRoute.Name
            );

            // Increment the call count by 1
            if (existingRoute != null) {
                existingRoute.Points += 1;
            }

            return existingRoute;
        }
    }
}
