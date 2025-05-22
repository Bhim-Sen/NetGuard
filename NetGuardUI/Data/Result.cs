using Newtonsoft.Json;

namespace NetGuardUI.Data
{
    public class Result
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        public string ReferralCode { get; set; }
    }
}
