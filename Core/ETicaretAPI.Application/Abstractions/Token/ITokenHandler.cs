using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Domain.Entites.Idetity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        TokenDTO CreateAccessToken(int accessTokenLifeTime,AppUser user);   //jwt=AccessToken=Token
        string CreateRefreshToken();
    }
}
