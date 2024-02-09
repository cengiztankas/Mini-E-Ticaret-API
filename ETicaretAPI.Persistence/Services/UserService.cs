using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Application.exceptions;
using ETicaretAPI.Application.Features.Commands.AppUserSQRS.CreateUser;
using ETicaretAPI.Application.Helpers;
using ETicaretAPI.Application.Repositories.EndpointT;
using ETicaretAPI.Domain.Entites;
using ETicaretAPI.Domain.Entites.Idetity;
using ETicaretAPI.Persistence.Repositories.EndpointT;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<AppUser>  _userManager;
        readonly RoleManager<AppRole> _roleManager;
        readonly IEndpointReadRepository _endpointReadRepository;

        public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IEndpointReadRepository endpointReadRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _endpointReadRepository = endpointReadRepository;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new ()
            {
                nameSurname = model.nameSurname,
                UserName = model.userName,
                Email = model.email
            }, model.password);
            CreateUserResponse response = new() { Success = result.Succeeded };
            if (result.Succeeded)
                response.Message = "Kayıt Başarılı";
            else
                foreach (var err in result.Errors)
                    response.Message += $"{err.Code} - {err.Description} \n";
            return response;
        }

      

        public async Task UpdateRefreshToken(string refreshToken, AppUser user, int addOnAccessTokenDate, DateTime accessTokenDate)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddSeconds(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
            }
            else
            {
                throw new NotFoundUserExeption();
            }
        }
        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
          AppUser? user=await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                resetToken = resetToken.UrlDecode();
                IdentityResult result= await _userManager.ResetPasswordAsync(user, resetToken,newPassword);
                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                }
                else
                {
                   throw new PasswordChangeFailedException();
                }
            }
        }

        public async Task<List<ListUserDTO>> GetAllUsersAsync(int page, int size)
        {
          var user=await _userManager.Users.Skip(page*size).Take(size).ToListAsync();
            return user.Select(c =>new ListUserDTO { 
                Id=c.Id,
                Email=c.Email,
                NameSurname=c.nameSurname,
                TwoFactorEnabled=c.TwoFactorEnabled,
                UserName=c.UserName
            }).ToList();
        }

 

        public int TotalUsersCount => _userManager.Users.Count();

        public async Task AssignRoleToUserAsnyc(string userId, string[] roles)
        {
          AppUser user=await _userManager.FindByIdAsync(userId);
           if(user != null)
            {
               var userRoles=await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRolesAsync(user,roles);

                
            }
        }

        public async Task<string[]> GetRolesToUserAsync(string UserId)
        {
            if (UserId != null)
            {
                var user = await _userManager.FindByIdAsync(UserId);
                if (user == null)
                {
                    user=await _userManager.FindByNameAsync(UserId);
                }
                if(user != null)
                {
                  var roles = await _userManager.GetRolesAsync(user);
                    return roles.ToArray();

                }

            
            }
            return new string[] { };
        }

        public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
        {
            var userRoles = await GetRolesToUserAsync(name);
            if (!userRoles.Any())
            {
                return false;
            }
            Endpoint? endpoint=await _endpointReadRepository.Table.Include(c=>c.Roles).FirstOrDefaultAsync(c=>c.Code==code);
            if (endpoint == null)
                return false;
            var endPointRoles=endpoint.Roles.Select(c=>c.Name).ToArray();
            foreach (var userRole in userRoles)
            {
                foreach (var endpointRole in endPointRoles)
                    if (userRole == endpointRole)
                        return true;
            }

            return false;
        }
    }
}
