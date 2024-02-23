using System;
using System.Threading.Tasks;
using FluentAssertions;
using Functional;
using Hub.Shell.Api.Controllers;
using Hub.Shell.Contracts;
using Hub.Shell.Domain.Services;
using Hub.Shell.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Hub.Shell.Api.Test.Controllers
{
    public class AuthorizationControllerTests
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthorizationController> _logger;
        private readonly AuthorizationController _sut;

        public AuthorizationControllerTests()
        {
            _authService = Substitute.For<IAuthService>();
            _logger = Substitute.For<ILogger<AuthorizationController>>();
            _sut = new AuthorizationController(_authService, _logger);
        }

        [Fact]
        public async Task Get_Token_By_Code()
        {
            var mockAuthTokenContract = new AuthorizationTokenContract
            {
                ExpiresUtc = new DateTime(2020, 12, 31),
                RefreshToken = "refresh_token",
                Value = "token"
            };
            _authService.GetTokenByAuthorizationCode(Arg.Any<string>(), Arg.Any<string>()).Returns(Result.Success<AuthorizationTokenContract, ServiceError>(mockAuthTokenContract));

            var result = await _sut.Code("localhost:3000", new AuthorizationCodeContract { Code = "code" });

            result.Should().BeEquivalentTo(new OkObjectResult(mockAuthTokenContract));
        }

        [Fact]
        public async Task Get_Token_By_Code_Failure()
        {
            var mockServiceError = new ServiceError(ErrorType.BadRequestData, "error");
            _authService.GetTokenByAuthorizationCode(Arg.Any<string>(), Arg.Any<string>()).Returns(Result.Failure<AuthorizationTokenContract, ServiceError>(mockServiceError));

            var result = await _sut.Code("localhost:3000", new AuthorizationCodeContract { Code = "code" });

            result.Should().BeEquivalentTo(new BadRequestObjectResult(mockServiceError.Message));
        }

        [Fact]
        public async Task Get_Token_By_Refresh_Token()
        {
            var mockAuthTokenContract = new AuthorizationTokenContract
            {
                ExpiresUtc = new DateTime(2020, 12, 31),
                RefreshToken = "refresh_token",
                Value = "token"
            };
            _authService.GetTokenByRefreshToken(Arg.Any<string>()).Returns(Result.Success<AuthorizationTokenContract, ServiceError>(mockAuthTokenContract));

            var result = await _sut.Refresh(new AuthorizationRefreshTokenContract { RefreshToken = "refresh_token" });

            result.Should().BeEquivalentTo(new OkObjectResult(mockAuthTokenContract));
        }

        [Fact]
        public async Task Get_Token_By_Refresh_Token_Failure()
        {
            var mockServiceError = new ServiceError(ErrorType.BadRequestData, "error");
            _authService.GetTokenByRefreshToken(Arg.Any<string>()).Returns(Result.Failure<AuthorizationTokenContract, ServiceError>(mockServiceError));

            var result = await _sut.Refresh(new AuthorizationRefreshTokenContract { RefreshToken = "refresh_token" });

            result.Should().BeEquivalentTo(new BadRequestObjectResult(mockServiceError.Message));
        }
    }
}