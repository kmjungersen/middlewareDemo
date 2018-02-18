using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TestApp.Web.Controllers;
using Xunit;
using static TestApp.Web.Controllers.TestRouteController;

namespace TestApp.Tests
{
    public class ApiTestRouteControllerTests
    {
        public string[] _baseRoutes;
        public TestRouteController _controller;
        public IEnumerable<Route> _routes;

        public ApiTestRouteControllerTests()
        {
            _baseRoutes = new string[] {
                "Foo", "Bar", "Baz"
            };

            var _controller = new TestRouteController();
            _controller.SetBaseRoutes(_baseRoutes);

            this._routes = _controller.GetRoutes();
        }

        [Fact]
        public void GetRoutes_ReturnsListOfTestRoutes()
        {
            var testRoute =  new Route {
                Name = _baseRoutes[0],
            };

            Assert.True(this._routes.Any());
            Assert.True(this._routes.Any(x => x.Name == testRoute.Name));
            // Assert.Contains(testRoute, this._routes);

            Assert.True(this._baseRoutes.Length == this._routes.Count());
        }
    }
}
