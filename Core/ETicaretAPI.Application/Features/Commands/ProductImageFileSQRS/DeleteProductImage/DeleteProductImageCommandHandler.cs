using ETicaretAPI.Application.Repositories.ProductT;
using ETicaretAPI.Domain.Entites;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFileSQRS.DeleteProductImage
{
    public class DeleteProductImageCommandHandler : IRequestHandler<DeleteProductImageCommandRequest, DeleteProductImageCommandResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductWriteRepository _productWriteRepository;

        public DeleteProductImageCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<DeleteProductImageCommandResponse> Handle(DeleteProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            Product? product = await _productReadRepository.Table.Include(c => c.ProductImages).FirstOrDefaultAsync(c => c.Id == Guid.Parse(request.id));
            ProductImageFile productImageFile = product.ProductImages.FirstOrDefault(c => c.Id == Guid.Parse(request.imageId));
            product.ProductImages.Remove(productImageFile);
            await _productWriteRepository.SaveAsync();
            return new();
        }
    }
}
