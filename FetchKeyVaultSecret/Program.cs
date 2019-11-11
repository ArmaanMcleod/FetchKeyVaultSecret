using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using FetchKeyVaultSecret.Services;

namespace FetchKeyVaultSecret
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")))
            {
                throw new FileNotFoundException("appsettings.json not found in current solution");
            }

            // Load in configuration settings from config file
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var authSettings = configurationBuilder.GetSection("AzureADAuthSettings");
            var keyVaultSettings = configurationBuilder.GetSection("KeyVaultSettings");

            var token = await TokenService.GetAuthTokenAsync(authSettings);

            var secret = await TokenService.GetKeyVaultSecretAsync(token, keyVaultSettings);

            Console.WriteLine($"Your secret is {secret}");
        }
    }
}
