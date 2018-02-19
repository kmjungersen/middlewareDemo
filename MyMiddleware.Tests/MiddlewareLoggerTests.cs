using Xunit;

namespace MyMiddleware.Tests
{
    public class MiddlewareLoggerTests
    {
        private MiddlewareLogger _logger;

        public void MiddlewareLoggerTests()
        {
            this._logger = new MiddlewareLogger();
        }

        [Fact]
        public void HandleReques_ReturnsRequestLog()
        {
            // Arrange
            var request = new Mock<HttpRequest>();
            var now = DateTime.Now;

        }
    }
}
