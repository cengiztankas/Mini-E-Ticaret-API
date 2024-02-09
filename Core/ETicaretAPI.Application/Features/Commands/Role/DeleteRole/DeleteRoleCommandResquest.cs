﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.Role.DeleteRole
{
    public class DeleteRoleCommandResquest:IRequest<DeleteRoleCommandResponse>
    {
        public string Id { get; set; }
    }
}
