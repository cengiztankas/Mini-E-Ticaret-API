using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.DTOs.facebook
{
    public class facebookUserAccessTokenValidaton
    {
        [JsonPropertyName("data")]
        public facebookUserAccessTokenValidatonData Data { get; set; }
    }
    public class facebookUserAccessTokenValidatonData
    {
         [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }
}
