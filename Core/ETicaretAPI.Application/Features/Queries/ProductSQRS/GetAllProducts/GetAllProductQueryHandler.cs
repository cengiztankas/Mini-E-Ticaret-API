using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.Products;
using ETicaretAPI.Application.Features.Commands.ProductSQRS.UpdateProduct;
using ETicaretAPI.Application.Repositories.ProductT;
using ETicaretAPI.Application.RequestParamerts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.ProductSQRS.GetAllProducts
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {
      readonly  IProductService _productService;
        readonly ILogger<GetAllProductQueryHandler> _logger;
        public GetAllProductQueryHandler(IProductReadRepository productReadRepository, ILogger<GetAllProductQueryHandler> logger, IProductService productService)
        {

            _logger = logger;
            _productService = productService;
        }

        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            //   _logger.LogInformation("ürünler getirildi");

            //var TotalProdcutCount = _productReadRepository.GetAll().Count();
            //var products = _productReadRepository.GetAll(false).Skip(request.page * request.size).Take(request.size)
            //    .Include(p=>p.ProductImages)
            //    .Select(c => new
            //{
            //    c.Id,
            //    c.Name,
            //    c.Stok,
            //    c.Price,
            //    c.CreatedDate,
            //    c.UpdatedDate,
            //    c.ProductImages
            //}).ToList();

            //return new()
            //{
            //    Products = products,
            //    TotalProductCount = TotalProdcutCount
            //};
          ResponseProduct product=await  _productService.GetProductsAsync(request.size, request.page);
            return new()
            {
                Products = product.Products,
                TotalProductCount = product.TotalProductCount,
            };
        }





    }
}
