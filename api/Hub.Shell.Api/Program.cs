using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Hub.Shell.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddEnvironmentVariables(); // For k8s ConfigMap

                    var builtConfig = config.Build();
                    var keyVaultUrl = builtConfig["AzureKeyVaultUrl"];
                    var accessToken = builtConfig["AZURE_TOKEN"];

                    // If a token is provided in env vars for the container, use it. Otherwise, use managed identity.
                    // NOTE: Token should only be used for local dev'ing.
                    var keyVaultClient = string.IsNullOrEmpty(accessToken) ?
                        new KeyVaultClient(
                            new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider()
                                .KeyVaultTokenCallback)) :
                        new KeyVaultClient((authority, resource, scope) => Task.FromResult(accessToken));

                    config.AddAzureKeyVault(
                        keyVaultUrl,
                        keyVaultClient,
                        new DefaultKeyVaultSecretManager());

                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        // This is technically already applied automatically, but we want it applied AFTER key-vault so do it again
                        config.AddJsonFile("appsettings.Development.json", true);
                    }
                });
    }
}
