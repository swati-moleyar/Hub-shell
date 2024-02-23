using System;
using Microsoft.Extensions.Configuration;

namespace Hub.Shell.Configuration
{
    public static class ConfigurationExtensions
    {
        public static TypedConfigurationGetter Get(this IConfiguration configuration)
        {
            return new TypedConfigurationGetter(configuration);
        }
    }

    public class TypedConfigurationGetter
    {
        private readonly IConfiguration _configuration;

        public TypedConfigurationGetter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetValueAndThrowIfMissing(string name) => _configuration[name] ?? throw new ArgumentNullException($"Missing configuration value '{name}'");

        public string ClientId => GetValueAndThrowIfMissing("ClientId");
        public string ClientSecret => GetValueAndThrowIfMissing("ClientSecret");
        public string Environment => GetValueAndThrowIfMissing("Environment");
        public string AccountsServiceUrl => GetValueAndThrowIfMissing("AccountsServiceUrl");
        public string UserManagerServiceUrl => GetValueAndThrowIfMissing("UserManagerServiceUrl");
        public string EntityStoreServiceUrl => GetValueAndThrowIfMissing("EntityStoreServiceUrl");
        public string FeatureTogglesServiceUrl => GetValueAndThrowIfMissing("FeatureTogglesServiceUrl");
        public string LegacyHubUrl => GetValueAndThrowIfMissing("LegacyHubUrl");
    }
}
