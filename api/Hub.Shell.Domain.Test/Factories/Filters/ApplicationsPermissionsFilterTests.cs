using System.Collections.Generic;
using Hub.Shell.Domain.Factories.Filters;
using Hub.Shell.External.UserManager;
using Hub.Shell.External.UserManager.Resources;
using Hub.Shell.External.Hub.Resources;
using Functional;
using FluentAssertions;
using NSubstitute;
using Xunit;


namespace Hub.Shell.Domain.Test.Factories.Filters
{
    public class ApplicationsPermissionsFilterTests
    {
        private readonly IUserManagerServiceClient _fakeUserManager;

        private readonly ApplicationsPermissionsFilter _sut;

        private const int UserId = 123;
        private const int EntityId = 456;
        private const int RoleId1 = 12;
        private const int RoleId2 = 34;
        private const int PermissionId1 = 56;
        private const int PermissionId2 = 78;

        public ApplicationsPermissionsFilterTests()
        {
            _fakeUserManager = Substitute.For<IUserManagerServiceClient>();

            _sut = new ApplicationsPermissionsFilter(_fakeUserManager);
        }

        [Fact]
        public async void Filters_By_Permissions()
        {
            var mockApplications = new List<ApplicationResource>
            {
                new ApplicationResource
                {
                    Id = "NullPermissions",
                    Permissions = null
                },
                new ApplicationResource
                {
                    Id = "HasAnyPermissions",
                    AssertionType = "Any",
                    Permissions = new string[] { "Role1Permission", "MissingPermission" }
                },
                new ApplicationResource
                {
                    Id = "MissingAnyPermissions",
                    AssertionType = "Any",
                    Permissions = new string[] { "MissingPermission" }
                },
                new ApplicationResource
                {
                    Id = "HasAllPermissions",
                    AssertionType = "All",
                    Permissions = new string[] { "Role1Permission", "Role2Permission" }
                },
                new ApplicationResource
                {
                    Id = "MissingAllPermissions",
                    AssertionType = "All",
                    Permissions = new string[] { "Role1Permission", "MissingPermission" }
                },
            };

            _fakeUserManager.GetSecurityRolesForUser(UserId).Returns(Result.Success<IEnumerable<AssignedSecurityRoleResource>, UserManagerError>
            (
                new List<AssignedSecurityRoleResource>
                {
                    new AssignedSecurityRoleResource { SecurityRoleId = RoleId1 },
                    new AssignedSecurityRoleResource { SecurityRoleId = RoleId2 }
                }
            ));

            _fakeUserManager.GetPermissionsForRole(EntityId, RoleId1).Returns(Result.Success<IEnumerable<PermissionResource>, UserManagerError>
            (
                new List<PermissionResource>
                {
                    new PermissionResource { Id = PermissionId1, Code = "Role1Permission" }
                }
            ));

            _fakeUserManager.GetPermissionsForRole(EntityId, RoleId2).Returns(Result.Success<IEnumerable<PermissionResource>, UserManagerError>
            (
                new List<PermissionResource>
                {
                    new PermissionResource { Id = PermissionId2, Code = "Role2Permission" }
                }
            ));

            var result = await _sut.Apply(mockApplications, UserId, EntityId);

            result.Success().ValueOrDefault().Should().BeEquivalentTo(new List<ApplicationResource>
            {
                new ApplicationResource
                {
                    Id = "NullPermissions",
                    Permissions = null
                },
                new ApplicationResource
                {
                    Id = "HasAnyPermissions",
                    AssertionType = "Any",
                    Permissions = new string[] { "Role1Permission", "MissingPermission" }
                },
                new ApplicationResource
                {
                    Id = "HasAllPermissions",
                    AssertionType = "All",
                    Permissions = new string[] { "Role1Permission", "Role2Permission" }
                }
            });
        }
    }
}
