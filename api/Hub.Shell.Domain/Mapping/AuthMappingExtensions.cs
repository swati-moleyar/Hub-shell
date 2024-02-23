using Hub.Shell.Contracts;
using Hub.Shell.Domain.Providers;
using Hub.Shell.External.Auth.Resources;

namespace Hub.Shell.Domain.Mapping
{
    public static class AuthMappingExtensions
    {
        public static AuthorizationTokenContract ToContract(this AuthResource resource, IDateTimeProvider dateTimeProvider)
        {
            return new AuthorizationTokenContract
            {
                ExpiresUtc = dateTimeProvider.UtcNow.AddSeconds(resource.ExpiresIn),
                RefreshToken = resource.RefreshToken,
                Value = resource.Token
            };
        }
    }
}