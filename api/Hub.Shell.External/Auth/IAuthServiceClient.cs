using System.Threading.Tasks;
using Functional;
using Hub.Shell.External.Auth.Resources;

namespace Hub.Shell.External.Auth
{
    public interface IAuthServiceClient
    {
        Task<Result<AuthResource, AuthError>> GetAuthResourceByCode(string authorizationCode, string redirectUri);
        Task<Result<AuthResource, AuthError>> GetAuthResourceByRefreshToken(string refreshToken);
        Task<Result<AuthenticationConfigurationResource, AuthError>> GetAuthenticationConfiguration(int companyId);
    }
}