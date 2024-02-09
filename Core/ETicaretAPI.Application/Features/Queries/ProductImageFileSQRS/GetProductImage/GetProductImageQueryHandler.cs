using ETicaretAPI.Application.exceptions;
using ETicaretAPI.Application.Repositories.ProductT;
using ETicaretAPI.Domain.Entites;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.ProductImageFileSQRS.GetProductImage
{
    public class GetProductImageQueryHandler : IRequestHandler<GetProductImageQueryRequest, List<GetProductImageQueryResponse>>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IConfiguration configuration;

        public GetProductImageQueryHandler(IProductReadRepository productReadRepository, IConfiguration configuration)
        {
            _productReadRepository = productReadRepository;
            this.configuration = configuration;
        }

        public async Task<List<GetProductImageQueryResponse>> Handle(GetProductImageQueryRequest request, CancellationToken cancellationToken)
        {
            Product? p = await _productReadRepository.Table.Include(p => p.ProductImages).FirstOrDefaultAsync(c => c.Id == Guid.Parse(request.id));
            //await Task.Delay(2000); //2 saniye bekliyor
          
            return  p.ProductImages.Select(p => new GetProductImageQueryResponse {
                Path = $"{configuration["BaseStorageUrl"]}/{p.Path}",
                fileName=    p.FileName,
                Id=p.Id,
                Showcase=p.Showcase,
            }).ToList();
        }
    }
}
