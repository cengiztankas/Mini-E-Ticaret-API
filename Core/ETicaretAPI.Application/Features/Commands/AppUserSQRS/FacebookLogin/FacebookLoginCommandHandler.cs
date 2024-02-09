using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.DTOs.facebook;
using ETicaretAPI.Domain.Entites.Idetity;
using MediatR;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ETicaretAPI.Application.Features.Commands.AppUserSQRS.FacebookLogin
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {

        readonly IAuthService _authService;
        readonly IConfiguration _configuration;
        public FacebookLoginCommandHandler(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }
        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {

            TokenDTO token = await _authService.FacebookLoginAsync(request.AuthToken, int.Parse(_configuration["TokenExpiredTime:accessTokenLifeTime"]));

            return new() { Token = token };
        }
    }
}
