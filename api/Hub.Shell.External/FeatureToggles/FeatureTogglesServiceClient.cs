using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Functional;
using Hub.Shell.Configuration;
using Hub.Shell.External.FeatureToggles.Resources;
using IQ.RestClient.Core;
using Microsoft.Extensions.Configuration;

namespace Hub.Shell.External.FeatureToggles
{
    public class FeatureTogglesServiceClient : IFeatureTogglesServiceClient
    {
        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;

        public FeatureTogglesServiceClient(IRestClient restClient, IConfiguration configuration)
        {
            _restClient = restClient;
            _configuration = configuration;
        }

        public async Task<Result<IEnumerable<FeatureToggleResource>, FeatureTogglesError>> GetEnabledFeatureToggles(int entityId)
        {
            var response = await _restClient.GetAsync($"{_configuration.Get().FeatureTogglesServiceUrl}/v1/enabled/entity({entityId})")
                .WithHeader("Accept", "application/json")
                .As<IEnumerable<FeatureToggleResource>, FeatureTogglesError>(CancellationToken.None);

            return response.Match(
                Result.Success<IEnumerable<FeatureToggleResource>, FeatureTogglesError>,
                error =>
                {
                    throw new Exception(error.Description);
                }
            );
        }

    }
}