using System;
using System.Threading.Tasks;
using FluentAssertions;
using Functional;
using Hub.Shell.Contracts;
using Hub.Shell.Domain.Providers;
using Hub.Shell.Domain.Services;
using Hub.Shell.Error;
using Hub.Shell.External.Auth;
using Hub.Shell.External.Auth.Resources;
using NSubstitute;
using Xunit;

namespace Hub.Shell.Domain.Test.Services
{
    public class AuthServiceTests
    {
        private readonly IAuthServiceClient _client;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly AuthService _sut;

        public AuthServiceTests()
        {
            _client = Substitute.For<IAuthServiceClient>();
            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _sut = new AuthService(_client, _dateTimeProvider);
        }

        [Fact]
        public async Task Get_Token_By_Code()
        {
            var mockAuthResource = new AuthResource
            {
                ExpiresIn = 43200,
                RefreshToken = "refresh_token",
                Token = "token"
            };
            _client.GetAuthResourceByCode(Arg.Any<string>(), Arg.Any<string>()).Returns(Result.Success<AuthResource, AuthError>(mockAuthResource));
            _dateTimeProvider.UtcNow.Returns(new DateTime(2000, 1, 1, 0, 0, 0));

            var result = await _sut.GetTokenByAuthorizationCode("code", "redirect_uri");

            result.Success().ValueOrDefault().Should().BeEquivalentTo(new AuthorizationTokenContract
            {
                ExpiresUtc = new DateTime(2000, 1, 1, 12, 0, 0),
                RefreshToken = mockAuthResource.RefreshToken,
                Value = mockAuthResource.Token
            });
        }

        [Fact]
        public async Task Get_Token_By_Code_Failure()
        {
            var mockAuthError = new AuthError
            {
                Message = "auth_error"
            };
            _client.GetAuthResourceByCode(Arg.Any<string>(), Arg.Any<string>()).Returns(Result.Failure<AuthResource, AuthError>(mockAuthError));

            var result = await _sut.GetTokenByAuthorizationCode("code", "redirect_uri");

            result.Failure().ValueOrDefault().Should().BeEquivalentTo(new ServiceError(ErrorType.BadRequestData, mockAuthError.Message));
        }

        [Fact]
        public async Task Get_Token_By_Refresh_Token()
        {
            var mockAuthResource = new AuthResource
            {
                ExpiresIn = 43200,
                RefreshToken = "refresh_token",
                Token = "token"
            };
            _client.GetAuthResourceByRefreshToken(Arg.Any<string>()).Returns(Result.Success<AuthResource, AuthError>(mockAuthResource));
            _dateTimeProvider.UtcNow.Returns(new DateTime(2000, 1, 1, 0, 0, 0));

            var result = await _sut.GetTokenByRefreshToken("refresh_token");

            result.Success().ValueOrDefault().Should().BeEquivalentTo(new AuthorizationTokenContract
            {
                ExpiresUtc = new DateTime(2000, 1, 1, 12, 0, 0),
                RefreshToken = mockAuthResource.RefreshToken,
                Value = mockAuthResource.Token
            });
        }

        [Fact]
        public async Task Get_Token_By_Refresh_Token_Failure()
        {
            var mockAuthError = new AuthError
            {
                Message = "auth_error"
            };
            _client.GetAuthResourceByRefreshToken(Arg.Any<string>()).Returns(Result.Failure<AuthResource, AuthError>(mockAuthError));

            var result = await _sut.GetTokenByRefreshToken("refresh_token");

            result.Failure().ValueOrDefault().Should().BeEquivalentTo(new ServiceError(ErrorType.BadRequestData, mockAuthError.Message));
        }
    }
}