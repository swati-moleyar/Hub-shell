using System.Threading.Tasks;
using Functional;
using Hub.Shell.Contracts;
using Hub.Shell.Domain.Mapping;
using Hub.Shell.Domain.Providers;
using Hub.Shell.Error;
using Hub.Shell.External.Auth;

namespace Hub.Shell.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthServiceClient _client;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AuthService(IAuthServiceClient client, IDateTimeProvider dateTimeProvider)
        {
            _client = client;
            _dateTimeProvider = dateTimeProvider;
        }

        public Task<Result<AuthorizationTokenContract, ServiceError>> GetTokenByAuthorizationCode(string authorizationCode, string redirectUri)
        {
            return _client.GetAuthResourceByCode(authorizationCode, redirectUri)
                .Match(
                    authResource => Result.Success<AuthorizationTokenContract, ServiceError>(authResource.ToContract(_dateTimeProvider)),
                    authError => Result.Failure<AuthorizationTokenContract, ServiceError>(new ServiceError(ErrorType.BadRequestData, authError.Message))
                );
        }

        public Task<Result<AuthorizationTokenContract, ServiceError>> GetTokenByRefreshToken(string refreshToken)
        {
            return _client.GetAuthResourceByRefreshToken(refreshToken)
                .Match(
                    authResource => Result.Success<AuthorizationTokenContract, ServiceError>(authResource.ToContract(_dateTimeProvider)),
                    authError => Result.Failure<AuthorizationTokenContract, ServiceError>(new ServiceError(ErrorType.BadRequestData, authError.Message))
                );
        }
    }
}
