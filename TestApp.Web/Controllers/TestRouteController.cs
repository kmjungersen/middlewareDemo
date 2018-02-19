using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TestApp.Web.Controllers
{
    [Route("api/[controller]")]
    public class TestRouteController : Controller
    {
        private static string[] _baseRoutes = new[]
        {
            "Vader", "Yoda", "Windu", "Rey", "Skywalker", "Kenobi", "Grievous", "Leia", "Solo"
        };

        private IEnumerable<Route> _routes;

        public TestRouteController()
        {
            if (this._routes == null)
            {
                this._routes = Enumerable.Range(0, _baseRoutes.Length).Select(x => new Route
                {
                    Name = _baseRoutes[x],
                    Points = 0,
                });
            }
        }

        public void SetBaseRoutes(string[] routes)
        {
            _baseRoutes = routes;
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
        public Route PostRoute(Route foo)
        {
            // var bar = HttpContext.Request.Body;
            var name = foo.Name;

            Console.WriteLine(name);
            var existingRoute = this._routes.FirstOrDefault(x =>
                x.Name == name
            );

            Route route;

            if (existingRoute == null) {
                route = new Route()
                {
                    Name = name,
                    Points = 1,
                    Blocked = true,
                };
                this._routes.Append(route);
            } else {
                existingRoute.Points += 1;
                route = existingRoute;
            }

            return route;
        }

        public class Route
        {
            public string Name { get; set; }
            public int? Points { get; set; }
            public bool Blocked { get; set; }
            // public SByte Data { get; set; }
        }
    }
}
