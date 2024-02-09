using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.OrderSQRS.CompleteOrder
{
    public class GetCompleteOrderQuertHandler : IRequestHandler<GetCompleteOrderQueryRequest, GetCompleteOrderQueryResponse>
    {
        readonly IOrderService _orderService;
        readonly IMailService _mailService;
        public GetCompleteOrderQuertHandler(IOrderService orderService, IMailService mailService)
        {
            _orderService = orderService;
            _mailService = mailService;
        }

        public async Task<GetCompleteOrderQueryResponse> Handle(GetCompleteOrderQueryRequest request, CancellationToken cancellationToken)
        {
            ( bool succeeded, CompletedOrderDTO dto)=await  _orderService.CompleteOrderAsync(request.Id);
            if (succeeded)
                await _mailService.SendCompletedOrderMailAsync(dto.EMail, dto.OrderCode, dto.OrderDate, dto.Username);
            return new() { }; 
        }
    }
}
