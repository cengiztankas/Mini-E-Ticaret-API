﻿using ETicaretAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AppUserSQRS.LoginUser
{
    public class LoginUserCommandResponse
    {
        public TokenDTO  token { get; set; }
    }
}
