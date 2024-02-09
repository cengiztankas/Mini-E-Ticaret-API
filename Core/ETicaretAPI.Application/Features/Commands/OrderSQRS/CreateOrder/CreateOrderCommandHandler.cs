using ETicaretAPI.Application.Abstractions.Hubs;
using ETicaretAPI.Application.Abstractions.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.OrderSQRS.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        readonly IOrderService _orderService;
        readonly IBasketService _basketService;
        readonly IOrderHubService _orderHubService;
        readonly IMailService _mailService;

        public CreateOrderCommandHandler(IOrderService orderService, IBasketService basketService, IOrderHubService orderHubService, IMailService mailService)
        {
            _orderService = orderService;
            _basketService = basketService;
            _orderHubService = orderHubService;
            _mailService = mailService;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            await _orderService.CreateOrderAsync(new()
            {
                Description = request.Description,
                Address = request.Address,
                BasketId = _basketService.GetUserActiveBasket?.Id.ToString()
            });
            await _orderHubService.OrderAddedMessageAsync("Heyy! , Yeni siparişiniz var");
            await _mailService.SendMailAsync("halismuhasibi.3810@gmail.com", "siparişiniz oluşturuldu", "merhaba Şiparişiniz oluşturldu. en kısa zamanda kargoya verilecektir.");
            return new();
        }
    }
}
