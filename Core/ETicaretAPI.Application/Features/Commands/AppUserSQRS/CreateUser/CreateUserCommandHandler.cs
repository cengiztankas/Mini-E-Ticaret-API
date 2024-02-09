using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.User;
using ETicaretAPI.Domain.Entites.Idetity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUserSQRS.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly IUserService _userServices;

        public CreateUserCommandHandler(IUserService userServices)
        {
            _userServices = userServices;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
           CreateUserResponse response= await _userServices.CreateAsync(new() { 
             userName=request.userName,
             nameSurname=request.nameSurname,
             email=request.email,
             password=request.PasswordGroup.password,
             passwordConfirm=request.PasswordGroup.passwordConfirm
            });

         return new()
         {
             Message = response.Message,
             Success = response.Success,
         };
        }



    }
}
