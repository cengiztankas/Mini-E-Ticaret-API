using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Domain.Entites;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.BasketSQRS.GetBasketItem
{
    public class GetBasketItemQueryHandler : IRequestHandler<GetBasketItemQueryRequest, List<GetBasketItemQueryResponse>>
    {
        readonly IBasketService _basketService;

        public GetBasketItemQueryHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<List<GetBasketItemQueryResponse>> Handle(GetBasketItemQueryRequest request, CancellationToken cancellationToken)
        {
            var basketItems = await _basketService.GetBasketItemsAsync();
         
                return basketItems.Select(ba => new GetBasketItemQueryResponse
                {
                    BasketItemId = ba.Id.ToString(),
                    Name = ba.Product.Name,
                    Price = ba.Product.Price,
                    Quantity = ba.Quantity
                }).ToList();
            
        }
    }
}
