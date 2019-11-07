using Newtonsoft.Json;

namespace FetchKeyVaultSecret.Models
{
    public class KeyVaultResponse
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }
    }

    public class Attributes
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("created")]
        public int Created { get; set; }

        [JsonProperty("updated")]
        public int Updated { get; set; }

        [JsonProperty("recoveryLevel")]
        public string RecoveryLevel { get; set; }
    }
}
