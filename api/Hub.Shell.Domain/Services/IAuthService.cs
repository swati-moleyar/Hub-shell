using System.Threading.Tasks;
using Functional;
using Hub.Shell.Contracts;
using Hub.Shell.Error;

namespace Hub.Shell.Domain.Services
{
    public interface IAuthService
    {
        Task<Result<AuthorizationTokenContract, ServiceError>> GetTokenByAuthorizationCode(string authorizationCode, string redirectUri);
        Task<Result<AuthorizationTokenContract, ServiceError>> GetTokenByRefreshToken(string refreshToken);
    }
}