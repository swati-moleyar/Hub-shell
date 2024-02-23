using System.Threading.Tasks;
using Functional;
using Hub.Shell.Contracts;
using Hub.Shell.Domain.Services;
using Hub.Shell.Error;
using IQ.AspNetCore.Auth.IoC.Microsoft;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hub.Shell.Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService, ILogger<SessionController> logger)
        {
            _logger = logger;
            _sessionService = sessionService;
        }

        [HttpGet]
        [Route("[action]")]
        [AuthorizeToken]
        public Task<IActionResult> Session([FromHeader] string origin)
        {
            return _sessionService.GetSession(origin)
                .Match<SessionContract, ServiceError, IActionResult>(
                    success => new OkObjectResult(success),
                    error => new BadRequestObjectResult(error.Message)
                );
        }
    }
}