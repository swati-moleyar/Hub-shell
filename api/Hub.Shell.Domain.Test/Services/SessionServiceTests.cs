using System.Threading.Tasks;
using System.Collections.Generic;
using FluentAssertions;
using Functional;
using Hub.Shell.Contracts;
using Hub.Shell.Domain.Services;
using Hub.Shell.Domain.Factories;
using Hub.Shell.Error;
using Hub.Shell.External.Auth;
using Hub.Shell.External.Auth.Resources;
using Hub.Shell.External.UserManager;
using Hub.Shell.External.UserManager.Resources;
using IQ.AspNetCore.Auth.IoC.Microsoft.AccessToken;
using IQ.Auth.OAuth2.AccessTokenFormatting;
using NSubstitute;
using Xunit;
using IQ.Auth.OAuth2.ProtectedResource.User;
using Hub.Shell.External.EntityStore;
using Hub.Shell.External.EntityStore.Resources;

namespace Hub.Shell.Domain.Test.Services
{
    public class SessionServiceTests
    {
        private readonly IProvideAuthToken _fakeAuthTokenProvider;
        private readonly IGetCurrentUserAsync _fakeAccessTokenDataExtractor;
        private readonly IAuthServiceClient _fakeAuthClient;
        private readonly IUserManagerServiceClient _fakeUserManagerClient;
        private readonly IApplicationsFactory _fakeApplicationsFactory;
        private readonly IEntityStoreServiceClient _fakeEntityStore;
        private readonly SessionService _sut;

        const int UserId = 123;
        const int ParentEntityId = 456;
        const string ParentEntityRole = "Company";
        const string Origin = "origin";
        const string Token = "this is a token";
        const string CovaLogo = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAFAAAAAVCAYAAADRhGlyAAAABGdBTUEAALGPC/xhBQAAA7BJREFUWAntmNtLVEEcx12t6GZRlGVWJBiUGRURkVhSEUQ9dCXoqiQ99AdESSEYVK9B0FPhQ0QQJEZCQUgZPYbdjG7Uk4hd6F5WZtvnKzPH8bjn7O5pfRD2B5+d38z8ft8zM+fM2dnNycladgWyKzCMVyBmxx6PxwvxN8IGGAdxSGaKORqLxdrcQLQmUF8HW2Eq/IUw66WzFa6i9dwNRGsM9UoYBb+hlZhuylAjr5iAhSaowz9GtROzgGITVEAuhJn6u6ARbqL33QtGaDt0QBSr8oRwEFgGj6MIkfMFasG9saXU/4I1TTqpEXzCJlDechOo50I9fIMo1kbSImlKaB5lAxSpIYJ5dw6tSeRfhLIIOkrJh5OgJ9eaFvOPqfTYxhTKuBPj3wG76asD7bQotoSkC8w3fwSOBjzeqHymvAY/wXsKTF9Q8dDpqMSfa+rabs3wAcK0NNE8WAuzQVYFV/q8ofmQvrXXOLdBixw2TvVPA73itG56PZTL2QzWWtjbe20lQjnDyXmJ1janHupyN48QcMoETadOekyLm1FDV3MucERPc50zTj3QNblPCSgxQYXafu6qp7NFAi9kOnrMBZPF2f5MXttqJird+ao/netq8fWF55n3/jItfnEvMIKTrla68RGG9N8pg3aEVnS42aBJBEwg1bg8dop/JyaSlJ7e1QNsuC2gnlL/rhkwIafiTjZsMQ+Tsx9S2QG69hzwbDgsoL7Fv8Jk0Hh1QG+HQOOJ0qF7jRPQ6fh+dxYNIpKlejcjiWcoSaf/O46WDsA1QV9QtBcR2wDLnZwmx8+om+kn0N0GOobYA3Aqg9Y5a5DpKIPV07EaJoIO2+dAi6iykZhP+KX4VbAH3OOUzqI62wbZWToU4449KFZPto48M22AFvAGrDcNFQzkGH43pCKod0sTE3hl8t+YUkUJWjrXvYcwLWnofbUTrL3TwtkK/gO0tDANMMW0r6AU+un3gnIV2B8EuH3WwmcN+WFHlfv0XzfxSQszp/44GhbDL4hq1VYNgQKI+pvavf4+q+mWBJRBE+iMGWZddNbBWDdfPm0j4RFYO+iPCaqTMBqe2UTK6lzdXRJqQT+9opi39dB6i4AG9DGKkMk5T3kpUT767aBfTitBcf7rPKHtECwl7jj8wE9k7o5w/USx/jY3vt9nNctB/zKka1v8V0BgPjSnKdRJ/A7oH5Rf2FcnthguQy8cgKR/DhCjP1DugrVdPtnAKgk6M96ziZTunx6BedmO7AoM3Qr8A6YWNTIefMrQAAAAAElFTkSuQmCC";
        const string IqmetrixLogo = "data:image/png;base64,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";

        public SessionServiceTests()
        {
            _fakeAuthTokenProvider = Substitute.For<IProvideAuthToken>();
            _fakeAccessTokenDataExtractor = Substitute.For<IGetCurrentUserAsync>();
            _fakeAuthClient = Substitute.For<IAuthServiceClient>();
            _fakeUserManagerClient = Substitute.For<IUserManagerServiceClient>();
            _fakeApplicationsFactory = Substitute.For<IApplicationsFactory>();
            _fakeEntityStore = Substitute.For<IEntityStoreServiceClient>();

            _sut = new SessionService(_fakeAuthTokenProvider, _fakeAccessTokenDataExtractor, _fakeAuthClient, _fakeUserManagerClient, _fakeApplicationsFactory, _fakeEntityStore);

            _fakeAuthTokenProvider.AuthToken.Returns(Token);
            _fakeAccessTokenDataExtractor.GetCurrentUserAsync(Arg.Any<string>()).Returns(new CurrentUserResult { UserId = UserId, ParentEntityId = ParentEntityId });
        }

        [Fact]
        public async Task Gets_Session()
        {
            var mockUserResource = new UserResource
            {
                Id = UserId,
                FirstName = "Poppy",
                LastName = "Blanche"
            };
            var mockAuthConfigResource = new AuthenticationConfigurationResource
            {
                IsEnabled = false
            };
            var mockEntity = new EntityResource
            {
                Id = ParentEntityId,
                Name = "Goats R Us",
                Role = ParentEntityRole
            };
            var mockApplications = new List<ApplicationGroupContract>
            {
                new ApplicationGroupContract
                {
                    Name = "Users",
                    Icon = "blue-square",
                    DefaultApp = null,
                    Apps = new List<ApplicationContract>
                    {
                        new ApplicationContract
                        {
                            Id = "usermanager",
                            Name = "User Manager",
                            Description = "Manage the users",
                            Href = "/usermanager",
                            Version = 1
                        }
                    }
                }
            };
            _fakeUserManagerClient.GetUser(UserId).Returns(Result.Success<UserResource, UserManagerError>(mockUserResource));
            _fakeAuthClient.GetAuthenticationConfiguration(ParentEntityId).Returns(Result.Success<AuthenticationConfigurationResource, AuthError>(mockAuthConfigResource));
            _fakeEntityStore.GetEntity(ParentEntityId).Returns(Result.Success<EntityResource, EntityStoreError>(mockEntity));
            _fakeApplicationsFactory.GetApplications(UserId, ParentEntityId, ParentEntityRole, Origin).Returns(Result.Success<IEnumerable<ApplicationGroupContract>, ServiceError>(mockApplications));

            var result = await _sut.GetSession(Origin);

            await _fakeAuthClient.Received(1).GetAuthenticationConfiguration(Arg.Any<int>());
            await _fakeUserManagerClient.Received(1).GetUser(Arg.Any<int>());
            await _fakeApplicationsFactory.Received(1).GetApplications(UserId, ParentEntityId, ParentEntityRole, Origin);
            result.Success().ValueOrDefault().Should().BeEquivalentTo(new SessionContract
            {
                UserId = mockUserResource.Id,
                FirstName = mockUserResource.FirstName,
                LastName = mockUserResource.LastName,
                CompanyId = mockEntity.Id,
                CompanyName = mockEntity.Name,
                CanChangePassword = !mockAuthConfigResource.IsEnabled,
                ApplicationGroups = mockApplications,
                ParentEntityRole = mockEntity.Role
            }, opt => opt.Excluding(x => x.BrandingLogo));
        }

        [Fact]
        public async Task Get_Authentication_Configuration_Failure()
        {
            var mockUserResource = new UserResource
            {
                Id = UserId,
                FirstName = "Poppy",
                LastName = "Blanche",
            };
            var mockAuthError = new AuthError
            {
                Message = "auth error"
            };
            var mockEntity = new EntityResource
            {
                Id = ParentEntityId,
                Name = "Goats R Us",
                Role = ParentEntityRole
            };
            _fakeUserManagerClient.GetUser(UserId).Returns(Result.Success<UserResource, UserManagerError>(mockUserResource));
            _fakeAuthClient.GetAuthenticationConfiguration(ParentEntityId).Returns(Result.Failure<AuthenticationConfigurationResource, AuthError>(mockAuthError));
            _fakeEntityStore.GetEntity(ParentEntityId).Returns(Result.Success<EntityResource, EntityStoreError>(mockEntity));

            var result = await _sut.GetSession(Origin);

            await _fakeUserManagerClient.Received(1).GetUser(Arg.Any<int>());
            await _fakeAuthClient.Received(1).GetAuthenticationConfiguration(Arg.Any<int>());
            await _fakeApplicationsFactory.DidNotReceive().GetApplications(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>());
            result.Failure().ValueOrDefault().Should().BeEquivalentTo(new ServiceError(ErrorType.BadRequestData, mockAuthError.Message));
        }


        [Fact]
        public async Task Get_User_Failure()
        {
            var mockUserManagerError = new UserManagerError
            {
                Message = "User Manager Error"
            };
            var mockEntity = new EntityResource
            {
                Id = ParentEntityId,
                Name = "Goats R Us",
                Role = ParentEntityRole
            };
            _fakeUserManagerClient.GetUser(UserId).Returns(Result.Failure<UserResource, UserManagerError>(mockUserManagerError));
            _fakeEntityStore.GetEntity(ParentEntityId).Returns(Result.Success<EntityResource, EntityStoreError>(mockEntity));

            var result = await _sut.GetSession(Origin);

            result.Success().ValueOrDefault().Should().BeNull();
            result.Failure().ValueOrDefault().Should().BeEquivalentTo(new ServiceError(ErrorType.BadRequestData, mockUserManagerError.Message));
        }

        [Fact]
        public async Task Get_Applications_Failure()
        {
            var mockUserResource = new UserResource
            {
                Id = UserId,
                FirstName = "Poppy",
                LastName = "Blanche",
            };
            var mockAuthConfigResource = new AuthenticationConfigurationResource
            {
                IsEnabled = false
            };
            var mockServiceError = new ServiceError
            {
                Type = ErrorType.BadRequestData,
                Message = "service error"
            };
            var mockEntity = new EntityResource
            {
                Id = ParentEntityId,
                Name = "Goats R Us",
                Role = ParentEntityRole
            };
            _fakeUserManagerClient.GetUser(UserId).Returns(Result.Success<UserResource, UserManagerError>(mockUserResource));
            _fakeAuthClient.GetAuthenticationConfiguration(ParentEntityId).Returns(Result.Success<AuthenticationConfigurationResource, AuthError>(mockAuthConfigResource));
            _fakeApplicationsFactory.GetApplications(UserId, ParentEntityId, ParentEntityRole, Origin).Returns(Result.Failure<IEnumerable<ApplicationGroupContract>, ServiceError>(mockServiceError));
            _fakeEntityStore.GetEntity(ParentEntityId).Returns(Result.Success<EntityResource, EntityStoreError>(mockEntity));

            var result = await _sut.GetSession(Origin);

            result.Success().ValueOrDefault().Should().BeNull();
            result.Failure().ValueOrDefault().Should().BeEquivalentTo(mockServiceError);
        }
        
        [Theory]
        [InlineData("https://hub.iqmetrix.net", IqmetrixLogo)]
        [InlineData(null, IqmetrixLogo)]
        [InlineData("not an actual origin", IqmetrixLogo)]
        [InlineData("https://hub.covasoft.net", CovaLogo)]
        public async Task Gets_Appropriate_Logo(string origin, string logo)
        {
            _fakeUserManagerClient.GetUser(UserId).ReturnsForAnyArgs(Result.Success<UserResource, UserManagerError>(new UserResource()));
            _fakeEntityStore.GetEntity(ParentEntityId).ReturnsForAnyArgs(Result.Success<EntityResource, EntityStoreError>(new EntityResource()));
            _fakeAuthClient.GetAuthenticationConfiguration(ParentEntityId).ReturnsForAnyArgs(Result.Success<AuthenticationConfigurationResource, AuthError>(new AuthenticationConfigurationResource()));
            _fakeApplicationsFactory.GetApplications(UserId, ParentEntityId, ParentEntityRole, origin).ReturnsForAnyArgs(Result.Success<IEnumerable<ApplicationGroupContract>, ServiceError>(new List<ApplicationGroupContract>()));

            var result = await _sut.GetSession(origin);

            result.Success().ValueOrDefault().BrandingLogo.Should().Be(logo);
        }
    }
}