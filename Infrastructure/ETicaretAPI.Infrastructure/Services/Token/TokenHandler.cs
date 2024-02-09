using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Domain.Entites.Idetity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenDTO CreateAccessToken(int accessTokenLifeTime, AppUser user)
        {
            TokenDTO token = new();
            //securiyKey in simetriğini alıyoruz
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            //şifrenlenmiş kimliği oluşturuyoruz.
            SigningCredentials _signingCredentials = new(securityKey,SecurityAlgorithms.HmacSha256);

            //Oluşturulacak Token ayarlarını veriyoruz
            token.Expiration=DateTime.UtcNow.AddSeconds(accessTokenLifeTime);

            JwtSecurityToken securityToken = new(
                        audience: _configuration["Token:Audience"],
                        issuer: _configuration["Token:Issuer"],
                        expires:token.Expiration,
                     //   notBefore:DateTime.UtcNow.AddMinutes(1)  //Token oluşturduktan bir dakika sonra aktif edilir
                     notBefore:DateTime.UtcNow,
                     signingCredentials:_signingCredentials,
                     claims:new List<Claim> { new (ClaimTypes.Name,user.UserName)}
                        );

            //Token Oluşturucu sınıfından bir örnek alalım.,
            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken=tokenHandler.WriteToken(securityToken);

            //CreateRefreshToken fonc Cağırdık
            token.RefreshToken = CreateRefreshToken();

            return token;
        }

        public string CreateRefreshToken()
        {
            byte[] number=new byte[32];
            using RandomNumberGenerator random=  RandomNumberGenerator.Create();
            random.GetBytes(number);
            return Convert.ToBase64String(number);
        }
    }
}
