using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TestApp.Web.Controllers;
using Xunit;

namespace TestApp.Tests
{
    public class ApiTestRouteControllerTests
    {
        [Fact]
        public void GetRoutes_ReturnsListOfTestRoutes()
        {
            var testRoutes = new string[] {
                "Foo", "Bar", "Baz"
            };

            var testRoutesEnum = (IEnumerable<string>)testRoutes;

            var controller = new TestRouteController(testRoutes);

            var result = controller.GetRoutes();

            Assert.Equal(testRoutes, result);
        }
    }
}
