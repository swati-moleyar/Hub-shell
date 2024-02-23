using System;
using FluentAssertions;
using Hub.Shell.Contracts;
using Hub.Shell.Domain.Mapping;
using Hub.Shell.Domain.Providers;
using Hub.Shell.External.Auth.Resources;
using NSubstitute;
using Xunit;

namespace Hub.Shell.Domain.Test.Mapping
{
    public class AuthMappingTests
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public AuthMappingTests()
        {
            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        }

        [Fact]
        public void Mapping_To_Contract()
        {
            var mockResource = new AuthResource
            {
                ExpiresIn = 43200,
                RefreshToken = "refresh_token",
                Token = "token"
            };
            _dateTimeProvider.UtcNow.Returns(new DateTime(2000, 1, 1, 0, 0, 0));

            var result = mockResource.ToContract(_dateTimeProvider);

            result.Should().BeEquivalentTo(new AuthorizationTokenContract
            {
                ExpiresUtc = new DateTime(2000, 1, 1, 12, 0, 0),
                RefreshToken = "refresh_token",
                Value = "token"
            });
        }
    }
}