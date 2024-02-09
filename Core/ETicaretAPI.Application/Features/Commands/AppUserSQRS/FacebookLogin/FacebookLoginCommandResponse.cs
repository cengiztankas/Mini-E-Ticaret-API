using ETicaretAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUserSQRS.FacebookLogin
{
    public class FacebookLoginCommandResponse
    {
        public TokenDTO Token { get; set; }

    }
}
