using System;
using System.Threading;
using System.Threading.Tasks;
using Functional;
using Hub.Shell.Configuration;
using Hub.Shell.External.Auth.Resources;
using Hub.Shell.External.RestClient;
using IQ.AspNetCore.Auth.IoC.Microsoft.AccessToken;
using IQ.RestClient.Core;
using Microsoft.Extensions.Configuration;

namespace Hub.Shell.External.Auth
{
    public class AuthServiceClient : IAuthServiceClient
    {
        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;
        private readonly IProvideAuthToken _authTokenProvider;

        public AuthServiceClient(IRestClient restClient, IConfiguration configuration, IProvideAuthToken authTokenProvider)
        {
            _restClient = restClient;
            _configuration = configuration;
            _authTokenProvider = authTokenProvider;
        }

        public async Task<Result<AuthResource, AuthError>> GetAuthResourceByCode(string authorizationCode, string redirectUri)
        {
            var post = _restClient.PostAsync($"{_configuration.Get().AccountsServiceUrl}/v1/oauth2/token", new
            {
                grant_type = "authorization_code",
                client_id = _configuration.Get().ClientId,
                client_secret = _configuration.Get().ClientSecret,
                code = authorizationCode,
                redirect_uri = redirectUri
            }, IQ.RestClient.Core.Common.Option.Some("application/x-www-form-urlencoded"));

            var response = await _restClient.Send<AuthResource, dynamic>(post.Request, CancellationToken.None);

            return response.Match(
                loginSuccess => Result.Success<AuthResource, AuthError>(loginSuccess),
                loginError =>
                {
                    if (loginError != null)
                    {
                        if (loginError.Message == "invalid_grant")
                        {
                            return Result.Failure<AuthResource, AuthError>(new AuthError { Message = loginError.Description });
                        }

                        throw new Exception(loginError.Description);
                    }
                    else
                    {
                        throw new Exception("Unknown login error");
                    }
                }
            );
        }

        public async Task<Result<AuthResource, AuthError>> GetAuthResourceByRefreshToken(string refreshToken)
        {
            var post = _restClient.PostAsync($"{_configuration.Get().AccountsServiceUrl}/v1/oauth2/token", new
            {
                grant_type = "refresh_token",
                client_id = _configuration.Get().ClientId,
                client_secret = _configuration.Get().ClientSecret,
                refresh_token = refreshToken
            }, IQ.RestClient.Core.Common.Option.Some("application/x-www-form-urlencoded"));

            var response = await _restClient.Send<AuthResource, AuthError>(post.Request, CancellationToken.None);

            return response.Match(
                Result.Success<AuthResource, AuthError>,
                loginError =>
                {
                    if (loginError.Message == "invalid_grant")
                    {
                        return Result.Failure<AuthResource, AuthError>(new AuthError { Message = loginError.Description });
                    }

                    throw new Exception(loginError.Description);
                }
            );
        }

        public async Task<Result<AuthenticationConfigurationResource, AuthError>> GetAuthenticationConfiguration(int companyId)
        {
            var get = _restClient.GetAsync($"{_configuration.Get().AccountsServiceUrl}/v1/entities({companyId})/authenticationConfiguration");

            var response = await _restClient.Send<AuthenticationConfigurationResource, AuthError>(get.Request, CancellationToken.None);

            return response.Match(
                Result.Success<AuthenticationConfigurationResource, AuthError>,
                loginError =>
                {
                    throw new Exception(loginError.Description);
                }
            );
        }
    }
}
