using Newtonsoft.Json;

namespace Demo.AppInsights.Api
{
    public class User
    {

        public string Name { get; set; }
        public string Email { get; set; }

        [Secure(ObfuscationType.MaskChar)]
        [JsonProperty("password")]
        public string Password { get; set;}

        [Secure(ObfuscationType.HeadVisible,6)]
        [JsonProperty("creditCard")]
        public string CreditCard { get; set;}
    }

}
