using ETicaretAPI.Application.Repositories.ProductT;
using ETicaretAPI.Domain.Entites;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.ProductSQRS.UpdateProduct
{
  
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        readonly IProductWriteRepository _productWriteRepository;
        readonly IProductReadRepository _productReadRepository;
        readonly Logger<UpdateProductCommandHandler> _logger;
        public UpdateProductCommandHandler(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository = null, Logger<UpdateProductCommandHandler> logger = null)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _logger = logger;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            Product p = await _productReadRepository.GetByIdAsync(request.Id);
            p.Name = request.Name;
            p.Stok = request.Stock;
            p.Price = request.Price;
            await _productWriteRepository.SaveAsync();
            _logger.LogInformation($"{p.Name} ürünü Güncellendi...");
            return new();
        }
    }
}
