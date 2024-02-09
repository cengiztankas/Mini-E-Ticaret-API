using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Role.GetAllRole
{
    public class GetAllRoleQueryRequest:IRequest<GetAllRoleQueryResponse>
    {
        public int Size { get; set; }
        public int Page { get; set; }
    }
}
