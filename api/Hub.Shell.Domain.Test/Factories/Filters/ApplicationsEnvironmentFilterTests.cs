using System.Collections.Generic;
using Hub.Shell.Configuration;
using Hub.Shell.Domain.Factories.Filters;
using Hub.Shell.External.Hub.Resources;
using Functional;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using NSubstitute;
using Xunit;


namespace Hub.Shell.Domain.Test.Factories.Filters
{
    public class ApplicationsEnvironmentFilterTests
    {
        private readonly IConfiguration _fakeConfiguration;

        private readonly ApplicationsEnvironmentFilter _sut;

        public ApplicationsEnvironmentFilterTests()
        {
            _fakeConfiguration = Substitute.For<IConfiguration>();

            _sut = new ApplicationsEnvironmentFilter(_fakeConfiguration);
        }

        [Fact]
        public void Filters_By_Environment()
        {
            _fakeConfiguration.Get().Environment.Returns("RC");

            var result = _sut.Apply(new List<ApplicationResource>
            {
                new ApplicationResource
                {
                    Id = "NullEnvrionment",
                    EnvironmentToggleLevel = null
                },
                new ApplicationResource
                {
                    Id = "DevEnvironment",
                    EnvironmentToggleLevel = "Dev",
                },
                new ApplicationResource
                {
                    Id = "IntEnvironment",
                    EnvironmentToggleLevel = "INT",
                },
                new ApplicationResource
                {
                    Id = "RcEnvironment",
                    EnvironmentToggleLevel = "rc",
                },
                new ApplicationResource
                {
                    Id = "ProdEnvironment",
                    EnvironmentToggleLevel = "ProD",
                },
                new ApplicationResource
                {
                    Id = "BadEnvironment",
                    EnvironmentToggleLevel = "bad",
                }
            });

            result.Success().ValueOrDefault().Should().BeEquivalentTo(new List<ApplicationResource>
            {
                new ApplicationResource
                {
                    Id = "NullEnvrionment",
                    EnvironmentToggleLevel = null
                },
                new ApplicationResource
                {
                    Id = "RcEnvironment",
                    EnvironmentToggleLevel = "rc",
                },
                new ApplicationResource
                {
                    Id = "ProdEnvironment",
                    EnvironmentToggleLevel = "ProD",
                }
            });
        }
    }
}
