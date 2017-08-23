using Newtonsoft.Json;

namespace PaySimpleSdk.Payments
{
    public class TokenResponse
    {
        [JsonProperty]
        public string JwtToken { get; set; }
    }
}