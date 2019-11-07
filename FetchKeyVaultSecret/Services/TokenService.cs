using FetchKeyVaultSecret.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FetchKeyVaultSecret.Services
{
    public class TokenService
    {
        private const string ResourceUri = "https://vault.azure.net";

        /// <summary>
        /// Fetches auth token from OAuth endpoint.
        /// </summary>
        /// <param name="authSettings">The OAuth setings in Azure AD</param>
        /// <returns>The token string</returns>
        public static async Task<string> GetAuthTokenAsync(IConfigurationSection authSettings)
        {
            var tenantId = authSettings.GetSection("tenantId").Value;

            // Auth endpoint for distrubuting tokens
            var url = $"https://login.microsoftonline.com/{tenantId}/oauth2/token?api-version=1.0";

            // Create POST data
            var postData = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "resource", ResourceUri },
                { "client_id", authSettings.GetSection("appId").Value },
                { "client_secret", authSettings.GetSection("password").Value }
            };

            using var httpClient = new HttpClient();

            // POST data to endpoint
            var response = await httpClient.PostAsync(url, new FormUrlEncodedContent(postData));

            // Only return token if 200 OK
            if (response.IsSuccessStatusCode)
            {
                // Read content into JSON string
                var json = await response.Content.ReadAsStringAsync();

                // Deserialize JSON into response object
                var deserializedJson = JsonConvert.DeserializeObject<AuthResponse>(json);

                return deserializedJson.AccessToken;
            }

            return string.Empty;
        }

        /// <summary>
        /// Fetches Key vault secret using Bearer access token
        /// </summary>
        /// <param name="token">Bearer access token</param>
        /// <param name="keyVaultSettings">Key vault settings needed</param>
        /// <returns>The key vault value</returns>
        public static async Task<string> GetKeyVaultSecretAsync(string token, IConfigurationSection keyVaultSettings)
        {
            var keyVaultName = keyVaultSettings.GetSection("keyVaultName").Value;
            var secretName = keyVaultSettings.GetSection("secretName").Value;

            // Key vault endpoint for querying
            var url = $"https://{keyVaultName}.vault.azure.net/secrets/{secretName}?api-version=7.0";

            // Authorization headers needed to authenticate
            var headers = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {token}" }
            };

            using var httpClient = new HttpClient();

            // Add Bearer token value to headers
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // GET data from endpoint
            var request = await httpClient.GetAsync(url);

            // Only return key value if 200 OK
            if (request.IsSuccessStatusCode)
            {
                // Read content into JSON string
                var json = await request.Content.ReadAsStringAsync();

                // Deserialize JSON into response object
                var deserializedJson = JsonConvert.DeserializeObject<KeyVaultResponse>(json);

                return deserializedJson.Value;
            }

            return string.Empty;
        }
    }
}
