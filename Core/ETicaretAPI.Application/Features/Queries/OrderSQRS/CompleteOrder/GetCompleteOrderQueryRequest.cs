using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.OrderSQRS.CompleteOrder
{
    public class GetCompleteOrderQueryRequest:IRequest<GetCompleteOrderQueryResponse>
    {
        public string Id { get; set; }
    }
}
