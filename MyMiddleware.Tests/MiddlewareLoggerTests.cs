using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Moq;
using MyMiddleware.Objects;
using Xunit;

namespace MyMiddleware.Tests
{
    public class MiddlewareLoggerTests
    {
        private MiddlewareLogger _logger;

        public MiddlewareLoggerTests()
        {
            this._logger = new MiddlewareLogger();
        }

        [Fact]
        public void HandleRequest_ReturnsRequestLog()
        {
            // Arrange
            var request = new Mock<HttpRequest>();
            var testPath = "/foo";
            var testQS = "?bar=baz";

            var queryString = new QueryString(testQS);
            Console.WriteLine(queryString.ToString());

            request.Setup(x => x.Path).Returns(testPath);
            request.Setup(x => x.QueryString).Returns(queryString);

            var now = DateTime.Now;

            // Act
            var requestLog = this._logger.HandleRequest(request.Object, now);

            // Assert
            Assert.NotNull(requestLog);
            Assert.True(requestLog.RequestTime == now);
            Console.WriteLine(requestLog.Path);
            Assert.True(requestLog.Path == testPath + testQS);
        }

        [Fact]
        public void HandleResponse_ReturnsRequestLog()
        {
            // Arrange
            var response = new Mock<HttpResponse>();
            response.Setup(x => x.StatusCode).Returns(200);

            var initialRequestLog = new RequestLog
            {
                RequestTime = DateTime.Now
            };

            // Act
            var requestLog = this._logger.HandleResponse(response.Object, initialRequestLog);

            // Assert
            Assert.True(requestLog.ResponseTime > requestLog.RequestTime);
            Assert.NotNull(requestLog.TotalRequestTime);
            Assert.True(requestLog.StatusCode == 200);
        }

        [Fact]
        public void LogRequest_ReturnsLoggeddRequestAndAddsToLogs()
        {
            // Arrange
            var requestLog = new RequestLog
            {
                Path = "/Foo/Bar/Baz",
                RequestTime = DateTime.Now,
                ResponseTime = DateTime.Now,
            };

            // Act
            var result = this._logger.LogRequest(requestLog);
            var logList = this._logger.GetLogs();

            // Assert
            Assert.Equal(requestLog.Path, result.Path);
            Assert.Contains(result, logList);
        }
    }
}
