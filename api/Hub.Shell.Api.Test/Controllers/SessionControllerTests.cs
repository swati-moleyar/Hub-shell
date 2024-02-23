using System.Threading.Tasks;
using System.Collections.Generic;
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
    public class SessionControllerTests
    {
        private readonly ISessionService _fakeSessionService;
        private readonly ILogger<SessionController> _fakeLogger;

        private readonly SessionController _sut;

        public SessionControllerTests()
        {
            _fakeSessionService = Substitute.For<ISessionService>();
            _fakeLogger = Substitute.For<ILogger<SessionController>>();

            _sut = new SessionController(_fakeSessionService, _fakeLogger);
        }

        [Fact]
        public async Task Gets_Session()
        {
            var sessionContract = new SessionContract
            {
                CompanyId = 23,
                CompanyName = "Contigo",
                UserId = 999,
                FirstName = "Nintendo",
                LastName = "Switch",
                CanChangePassword = true,
                ApplicationGroups = new List<ApplicationGroupContract>
                {
                    new ApplicationGroupContract
                    {
                        Name = "Hub Group",
                        Icon = "green-circle",
                        DefaultApp = null,
                        Apps = new List<ApplicationContract>
                        { 
                            new ApplicationContract
                            {
                                Id = "hubmanager",
                                Name = "Hub Manager",
                                Description = "Manage Hub",
                                Href = "/hubmanager",
                                Version = 1
                            } 
                        }
                    }
                }
            };

            _fakeSessionService.GetSession("blah").Returns(Result.Success<SessionContract, ServiceError>(sessionContract));
            var result = await _sut.Session("blah");

            result.Should().BeEquivalentTo(new OkObjectResult(sessionContract));
        }

        [Fact]
        public async Task Get_Session_Fails()
        {
            var serviceError = new ServiceError(ErrorType.BadRequestData, "error");

            _fakeSessionService.GetSession("blah").Returns(Result.Failure<SessionContract, ServiceError>(serviceError));
            var result = await _sut.Session("blah");

            result.Should().BeEquivalentTo(new BadRequestObjectResult(serviceError.Message));
        }
    }
}