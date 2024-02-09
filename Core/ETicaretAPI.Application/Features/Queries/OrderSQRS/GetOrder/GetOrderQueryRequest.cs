using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.OrderSQRS.GetOrder
{
    public class GetOrderQueryRequest:IRequest<GetOrderQueryResponse>
    {
        public int page { get; set; } = 0;
        public int size { get; set; } = 5;
    }
}
