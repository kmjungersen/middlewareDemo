using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TestApp.Web.Controllers;
using Xunit;
using static TestApp.Web.Controllers.TestRouteController;
using TestApp.Web.Objects;
using MyMiddleware;
using MyMiddleware.Objects;
using Moq;
using Microsoft.Extensions.DependencyInjection;
using TestApp.Web;
using Microsoft.Extensions.Configuration;

namespace TestApp.Tests
{
    public class ApiLoggerControllerTests
    {
        public ApiLoggerControllerTests() { }

        [Fact]
        public void Startup_ProperlyRegisteringIMiddlewareLogger()
        {
            // Arrange
            IServiceCollection services = new ServiceCollection();
            Mock<IConfiguration> config = new Mock<IConfiguration>();

            var target = new Startup(config.Object);

            // Act
            target.ConfigureServices(services);
            services.AddSingleton<MiddlewareLogger>();

            // Assert
            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<MiddlewareLogger>();

            Assert.NotNull(logger);
        }
    }
}