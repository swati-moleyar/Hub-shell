using Newtonsoft.Json;

namespace Hub.Shell.External.Auth.Resources
{
    public class AuthResource
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
