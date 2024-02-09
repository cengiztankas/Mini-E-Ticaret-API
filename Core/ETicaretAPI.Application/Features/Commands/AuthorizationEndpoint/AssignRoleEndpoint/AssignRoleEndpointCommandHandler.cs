using ETicaretAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.AuthorizationEndpoint.AssignRoleEndpoint
{
    public class AssignRoleEndpointCommandHandler : IRequestHandler<AssignRoleEndpointCommandRequest, AssignRoleEndpointCommandResponse>
    {
        readonly IAuthorizationEndpointService _authorizationEndpoint;

        public AssignRoleEndpointCommandHandler(IAuthorizationEndpointService authorizationEndpoint)
        {
            _authorizationEndpoint = authorizationEndpoint;
        }

        public async Task<AssignRoleEndpointCommandResponse> Handle(AssignRoleEndpointCommandRequest request, CancellationToken cancellationToken)
        {
           await  _authorizationEndpoint.AssignRoleEndpointAsync(request.Roles, request.Menu, request.Code,request.Type);
            return new() { };
        }
    }
}
