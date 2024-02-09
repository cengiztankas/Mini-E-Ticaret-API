using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUserSQRS.FacebookLogin
{
    public class FacebookLoginCommandRequest:IRequest<FacebookLoginCommandResponse>
    {
        [JsonPropertyName("authToken")]
        public string AuthToken { get; set; }
    }
}
