using System.Linq;
using Hub.Shell.Configuration;
using Hub.Shell.External.Auth;
using Hub.Shell.External.EntityStore;
using Hub.Shell.External.Hub;
using Hub.Shell.External.RestClient;
using Hub.Shell.External.UserManager;
using Hub.Shell.External.FeatureToggles;
using IQ.RestClient;
using IQ.RestClient.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hub.Shell.External.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExternal(this IServiceCollection services)
        {
            services.TryAddSingleton<IRestClient>(service =>
            {
                var config = service.GetRequiredService<IConfiguration>();
                var restClientConfig = new DefaultConfiguration(new ServiceToken(config));

                // Add any extra deserializers here
                restClientConfig.Deserializers = restClientConfig.Deserializers.Concat(new[]
                {
                    new HalJsonDeserializer()
                }).ToArray();

                return new IQ.RestClient.Core.RestClient(restClientConfig);
            });

            services.TryAddScoped<IAuthServiceClient, AuthServiceClient>();
            services.TryAddScoped<IUserManagerServiceClient, UserManagerServiceClient>();
            services.TryAddScoped<IEntityStoreServiceClient, EntityStoreServiceClient>();
            services.TryAddScoped<IFeatureTogglesServiceClient, FeatureTogglesServiceClient>();
            services.TryAddScoped<IProvideApplications, ApplicationsProvider>();

            return services;
        }

        public class ServiceToken : ServiceTokenCredentials
        {
            private readonly IConfiguration _configuration;

            public ServiceToken(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public override string AccountsServiceUrl => _configuration.Get().AccountsServiceUrl;
            public override string ClientId => _configuration.Get().ClientId;
            public override string ClientSecret => _configuration.Get().ClientSecret;
        }
    }
}