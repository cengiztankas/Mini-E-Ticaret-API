using ETicaretAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.OrderSQRS.GetOrder
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQueryRequest, GetOrderQueryResponse>
    {
        readonly IOrderService _orderService;
public GetOrderQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrderQueryResponse> Handle(GetOrderQueryRequest request, CancellationToken cancellationToken)
        {
        var response=  await  _orderService.GetAllOrdersAsync(request.size,request.page);
            return new()
            {
                Orders=response.Orders,
                TotalOrdersCount = response.TotalOrdersCount,
            };
        }
    }
}
