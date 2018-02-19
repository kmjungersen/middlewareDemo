using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TestApp.Web.Controllers;
using Xunit;
using static TestApp.Web.Controllers.TestRouteController;
using TestApp.Web.Objects;

namespace TestApp.Tests
{
    public class ApiTestRouteControllerTests
    {
        public string[] _baseRoutes;
        public TestRouteController _controller;
        public IEnumerable<Route> _routes;

        public ApiTestRouteControllerTests()
        {
            this._controller = new TestRouteController();

            this._baseRoutes = TestRouteController._baseRoutes;

            this._routes = this._controller.GetRoutes();
        }

        [Fact]
        public void GetRoute_ReturnsRoute()
        {
            var testNameExists = this._baseRoutes[0];

            // Since this route should exist, the controller should return a route
            var result1 = this._controller.GetRoute(testNameExists);
            Assert.True(result1 != null);
            Assert.True(result1.Name == testNameExists);

            var testNameDNE = "Foo";

            // Since this route should NOT exist, the controller should NOT return a route
            var result2 = this._controller.GetRoute(testNameDNE);
            Assert.True(result2 == null);
        }

        [Fact]
        public void GetRoutes_ReturnsListOfTestRoutes()
        {
            var testRoute =  new Route
            {
                Name = this._baseRoutes[0],
            };

            // Verify contorller is returning any routes
            Assert.True(this._routes.Any());

            //Verify controller is returning a route we know should exist
            Assert.True(this._routes.Any(x => x.Name == testRoute.Name));

            // Verify that we got the right number of routes from the controller based on our starting list
            Assert.True(this._baseRoutes.Length == this._routes.Count());
        }

        [Fact]
        public void PostRoute_UpdatesExistingRoute()
        {
            var testRoute = new Route
            {
                Name = _baseRoutes[0],
            };

            // Fetch exisisting route to use as comparison for the next test
            var existingRoute = this._routes.FirstOrDefault(x => x.Name == testRoute.Name);

            // Verify that posting a route increments the Points by 1 each time
            var result = this._controller.PostRoute(testRoute);
            Assert.Equal(result.Points, existingRoute.Points + 1);
        }
    }
}
