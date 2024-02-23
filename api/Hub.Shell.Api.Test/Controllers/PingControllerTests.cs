using FluentAssertions;
using Hub.Shell.Api.Controllers;
using Hub.Shell.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Hub.Shell.Api.Test.Controllers
{
    public class PingControllerTests
    {
        private readonly ILogger<PingController> _logger;
        private readonly PingController _sut;

        public PingControllerTests()
        {
            _logger = Substitute.For<ILogger<PingController>>();
            _sut = new PingController(_logger);
        }

        [Fact]
        public void Ping()
        {
            var result = _sut.Ping();

            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().BeOfType<PingContract>();
            result.As<OkObjectResult>().Value.As<PingContract>().ApplicationVersion.Should().NotBeNullOrEmpty();
        }
    }
}