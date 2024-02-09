using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.DTOs.facebook
{
    public class facebookAccessTokenResponseDTO
    {
        [JsonPropertyName("access_token")]  //isimleri değiştiriyoruz
        public string AccessToken { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }
}
