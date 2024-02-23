using System.Threading.Tasks;
using Functional;
using Hub.Shell.Contracts;
using Hub.Shell.Domain.Services;
using Hub.Shell.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hub.Shell.Api.Controllers
{
    [ApiController, AllowAnonymous]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(
            IAuthService authService,
            ILogger<AuthorizationController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost]
        [Route("[action]")]
        public Task<IActionResult> Code([FromHeader] string origin, [FromBody] AuthorizationCodeContract contract)
        {
            return _authService.GetTokenByAuthorizationCode(contract.Code, origin)
                .Match<AuthorizationTokenContract, ServiceError, IActionResult>(
                    success => new OkObjectResult(success),
                    error => new BadRequestObjectResult(error.Message)
                );
        }

        [HttpPost]
        [Route("[action]")]
        public Task<IActionResult> Refresh([FromBody] AuthorizationRefreshTokenContract contract)
        {
            return _authService.GetTokenByRefreshToken(contract.RefreshToken)
                .Match<AuthorizationTokenContract, ServiceError, IActionResult>(
                    success => new OkObjectResult(success),
                    error => new BadRequestObjectResult(error.Message)
                );
        }
    }
}