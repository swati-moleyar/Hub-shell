using System;
using System.Threading;
using System.Threading.Tasks;
using Functional;
using Hub.Shell.Configuration;
using Hub.Shell.External.EntityStore.Resources;
using IQ.RestClient.Core;
using Microsoft.Extensions.Configuration;

namespace Hub.Shell.External.EntityStore
{
    public class EntityStoreServiceClient : IEntityStoreServiceClient
    {
        private readonly IRestClient _restClient;
        private readonly IConfiguration _configuration;

        public EntityStoreServiceClient(IRestClient restClient, IConfiguration configuration)
        {
            _restClient = restClient;
            _configuration = configuration;
        }

        public async Task<Result<EntityResource, EntityStoreError>> GetEntity(int entityId)
        {
            var response = await _restClient.GetAsync($"{_configuration.Get().EntityStoreServiceUrl}/v1/entities({entityId})")
                .As<EntityResource, EntityStoreError>(CancellationToken.None);

            return response.Match(
                Result.Success<EntityResource, EntityStoreError>,
                error =>
                {
                    throw new Exception(error.Description);
                }
            );
        }

    }
}