using Hub.Shell.Domain.Factories;
using Hub.Shell.Domain.Factories.Filters;
using Hub.Shell.Domain.Providers;
using Hub.Shell.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hub.Shell.Domain.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.TryAddScoped<IAuthService, AuthService>();
            services.TryAddScoped<ISessionService, SessionService>();

            services.TryAddScoped<IDateTimeProvider, DateTimeProvider>();

            services.TryAddScoped<IApplicationsFactory, ApplicationsFactory>();

            services.TryAddScoped<IApplicationsEnvironmentFilter, ApplicationsEnvironmentFilter>();
            services.TryAddScoped<IApplicationsPermissionsFilter, ApplicationsPermissionsFilter>();
            services.TryAddScoped<IApplicationsFeaturesFilter, ApplicationsFeaturesFilter>();

            return services;
        }
    }
}