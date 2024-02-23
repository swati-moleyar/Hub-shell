using System.Threading.Tasks;
using System.Collections.Generic;
using Functional;
using Hub.Shell.Domain.Factories;
using Hub.Shell.Domain.Factories.Filters;
using Hub.Shell.Contracts;
using Hub.Shell.Error;
using Hub.Shell.External.Hub;
using Hub.Shell.External.Hub.Resources;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Hub.Shell.Domain.Test.Factories
{
    public class ApplicationsFactoryTests
    {
        private readonly IProvideApplications _fakeApplications;
        private readonly IApplicationsEnvironmentFilter _fakeEnvironmentFilter;
        private readonly IApplicationsPermissionsFilter _fakePermissionsFilter;
        private readonly IApplicationsFeaturesFilter _fakeFeaturesFilter;
        private readonly ApplicationsFactory _sut;

        private readonly ApplicationsResource MockAppsResource = new ApplicationsResource()
        {
            ApplicationGroups = new ApplicationGroupResource[]
            {
                new ApplicationGroupResource
                {
                    GroupName = "DefaultApplicationGroup"
                }
            },
            ApplicationGroupVariations = new Dictionary<string, ApplicationGroupResource[]>
            {
                { 
                    EntityRole, new ApplicationGroupResource[] 
                    {
                        new ApplicationGroupResource
                        {
                            GroupName = "CompanyApplicationGroup1",
                            Applications = new string[] { "Group1App1", "Group1App2", "MissingApp" }
                        },
                        new ApplicationGroupResource
                        {
                            GroupName = "CompanyApplicationGroup2",
                            Applications = new string[] { "Group2App1", "Group2App2", "MissingApp" }
                        },
                        new ApplicationGroupResource
                        {
                            GroupName = "EmptyGroup",
                            Applications = new string[] { "MissingApp" }
                        }
                    }
                }
            },
            Applications = new Dictionary<string, ApplicationResource>
            {
                { "Group1App1", new ApplicationResource { Id = "Group1App1" } },
                { "Group1App2", new ApplicationResource { Id = "Group1App2" } },
                { "Group2App1", new ApplicationResource { Id = "Group2App1" } },
                { "Group2App2", new ApplicationResource { Id = "Group2App2" } },
                { "UngroupedApp", new ApplicationResource { Id = "UngroupedApp" } }
            }
        };

        const int UserId = 123;
        const int EntityId = 456;
        const string EntityRole = "Company";

        public ApplicationsFactoryTests()
        {
            _fakeApplications = Substitute.For<IProvideApplications>();
            _fakeEnvironmentFilter = Substitute.For<IApplicationsEnvironmentFilter>();
            _fakePermissionsFilter = Substitute.For<IApplicationsPermissionsFilter>();
            _fakeFeaturesFilter = Substitute.For<IApplicationsFeaturesFilter>();

            _sut = new ApplicationsFactory(_fakeApplications, _fakeEnvironmentFilter, _fakePermissionsFilter, _fakeFeaturesFilter);

            _fakeEnvironmentFilter.Apply(Arg.Any<IEnumerable<ApplicationResource>>()).Returns(x => 
                Result.Success<IEnumerable<ApplicationResource>, ServiceError>((IEnumerable<ApplicationResource>)x[0])
            );
            _fakePermissionsFilter.Apply(Arg.Any<IEnumerable<ApplicationResource>>(), UserId, EntityId).Returns(x => 
                Result.Success<IEnumerable<ApplicationResource>, ServiceError>((IEnumerable<ApplicationResource>)x[0])
            );
            _fakeFeaturesFilter.Apply(Arg.Any<IEnumerable<ApplicationResource>>(), EntityId).Returns(x => 
                Result.Success<IEnumerable<ApplicationResource>, ServiceError>((IEnumerable<ApplicationResource>)x[0])
            );
        }

        [Fact]
        public async Task Gets_Applications()
        {
            _fakeApplications.GetApplications().Returns(
                Result.Success<ApplicationsResource, string>(MockAppsResource)
            );

            var result = await _sut.GetApplications(UserId, EntityId, EntityRole, "origin");

            result.Success().ValueOrDefault().Should().BeEquivalentTo(new List<ApplicationGroupContract>
            {
                new ApplicationGroupContract
                {
                    Name = "CompanyApplicationGroup1",
                    Apps = new List<ApplicationContract> { 
                        new ApplicationContract { Id = "Group1App1", Version = 1 }, 
                        new ApplicationContract { Id = "Group1App2", Version = 1 }
                    }
                },
                new ApplicationGroupContract
                {
                    Name = "CompanyApplicationGroup2",
                    Apps = new List<ApplicationContract> { 
                        new ApplicationContract { Id = "Group2App1", Version = 1 }, 
                        new ApplicationContract { Id = "Group2App2", Version = 1 }
                    }
                }
            });
        }
    }
}