using System;
using System.Threading;
using System.Threading.Tasks;
using Hub.Shell.Configuration;
using Hub.Shell.External.Hub.Resources;
using Functional;
using IQ.RestClient.Core;
using Microsoft.Extensions.Configuration;

namespace Hub.Shell.External.Hub
{
    public class ApplicationsProvider : IProvideApplications
    {
        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;

        public ApplicationsProvider(IRestClient restClient, IConfiguration configuration)
        {
            _restClient = restClient;
            _configuration = configuration;
        }

        public async Task<Result<ApplicationsResource, string>> GetApplications()
        {
            var response = await _restClient.GetAsync($"{_configuration.Get().LegacyHubUrl}/applications.json")
                .As<ApplicationsResource, string>(CancellationToken.None);
            
            return response.Match(
                Result.Success<ApplicationsResource, string>,
                error =>
                {
                    throw new Exception(error);
                }
            );
        }
    }
}
