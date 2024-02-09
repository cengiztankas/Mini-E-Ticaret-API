
using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.facebook;
using ETicaretAPI.Application.exceptions;
using ETicaretAPI.Application.Helpers;
using ETicaretAPI.Domain.Entites.Idetity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace ETicaretAPI.Persistence.Services
{
    public class AuthService : IAuthService
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly IConfiguration _configuration;
        readonly ITokenHandler _tokenHandler;
        readonly HttpClient _httpClient;
        readonly IUserService _userService;
        readonly IMailService _mailService;
        public AuthService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration, UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, IUserService userService, IMailService mailService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _httpClient = httpClientFactory.CreateClient();
            _signInManager = signInManager;
            _userService = userService;
            _mailService = mailService;
        }

        async Task<TokenDTO> CreateUserExternalAsync(AppUser user, string email, string name, UserLoginInfo info, int accessTokenLifeTime)
        {
            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        nameSurname = name
                    };
                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (result) 
            {
                await _userManager.AddLoginAsync(user, info); //AspNetUserLogins

                TokenDTO token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
               await _userService.UpdateRefreshToken( token.RefreshToken, user,int.Parse(_configuration["TokenExpiredTime:refreshTokenLifeTime"]), token.Expiration); //refresh token ı update ediyoruz
                return token;
            }
            throw new Exception("Invalid external authentication.");
        }

        public async Task<TokenDTO> FacebookLoginAsync(string authToken, int accessTokenLifeTime)
        {
            string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:Client_ID"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:Client_Secret"]}&grant_type=client_credentials");

            facebookAccessTokenResponseDTO facebookAccessTokenResponse = JsonSerializer.Deserialize<facebookAccessTokenResponseDTO>(accessTokenResponse);
            string userAccessTokenValidaton =
            await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessTokenResponse.AccessToken}");
            facebookUserAccessTokenValidaton validation =
                 JsonSerializer.Deserialize<facebookUserAccessTokenValidaton>(userAccessTokenValidaton);
            if (validation.Data.IsValid)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");
                FacebookUserInfoResponse userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);



                var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");
                AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

                return await CreateUserExternalAsync(user, userInfo.Email, userInfo.Name, info, accessTokenLifeTime);
            }
            throw new Exception("Invalid external authentication.");
        }
        public async Task<TokenDTO> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
        {
            ValidationSettings? settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>()
              { _configuration["ExternalLoginSettings:Google:Client_ID"] }
            };
            Payload payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            UserLoginInfo info = new ("GOOGLE", payload.Subject, "GOOGLE");
            AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            return await CreateUserExternalAsync(user, payload.Email, payload.Name, info, accessTokenLifeTime);
        }

        public async Task<TokenDTO> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime)
        {
            AppUser user = await _userManager.FindByNameAsync(usernameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(usernameOrEmail);
            }

            if (user == null)
            {
                throw new NotFoundUserExeption();
            }
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {
                //token oluşturulacak
                TokenDTO token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, int.Parse(_configuration["TokenExpiredTime:refreshTokenLifeTime"]), token.Expiration); //refresh token ı update ediyoruz
                return token;
            }
            throw new AuthenticationErrorExcepiton();
        }

        public async Task<TokenDTO> RefreshTokenLoginAsync(string refreshToken)
        {
       AppUser? user=    await _userManager.Users.FirstOrDefaultAsync(c=>c.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                TokenDTO token = _tokenHandler.CreateAccessToken(int.Parse(_configuration["TokenExpiredTime:refreshTokenLifeTime"]), user);
                await _userService.UpdateRefreshToken(token.RefreshToken, user, int.Parse(_configuration["TokenExpiredTime:refreshTokenLifeTime"]), token.Expiration);
                return token;
            }
            else
            {
                throw new NotFoundUserExeption();
            }
        }

        public async Task PasswordResetAsnyc(string email)
        {
           AppUser user=await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
               string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                //byte[] tokenByte= Encoding.UTF8.GetBytes(resetToken); //utf8 e göre encode ediliyor
                //resetToken = WebEncoders.Base64UrlEncode(tokenByte);//web url e uygun olmayan karakterler düzeltiliyor.
                resetToken = resetToken.UrlEncode();
                await _mailService.SendPasswordResetMailAsync(email, user.Id,resetToken);
            }
        }

        public async Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                //byte[] tokenBytes = WebEncoders.Base64UrlDecode(resetToken);
                //resetToken = Encoding.UTF8.GetString(tokenBytes);
                resetToken = resetToken.UrlDecode();

                return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);
            }
                return false;
        }
    }
}
