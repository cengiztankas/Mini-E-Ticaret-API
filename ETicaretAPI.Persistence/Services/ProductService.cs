using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.DTOs.ProductImage;
using ETicaretAPI.Application.DTOs.Products;
using ETicaretAPI.Application.Repositories.ProductT;
using ETicaretAPI.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class ProductService : IProductService
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IQRCodeService _qrCodeService;
        public ProductService(IProductReadRepository productReadRepository, IQRCodeService qrCodeService)
        {
            _productReadRepository = productReadRepository;
            _qrCodeService = qrCodeService;
        }

        public async Task<ResponseProduct> GetProductsAsync(int size, int page)
        {
            var totalProductCount = _productReadRepository.GetAll().Count();
            var products = _productReadRepository.GetAll(false).Skip(size * page).Take(size)
                                    .Include(c => c.ProductImages)
                                     .Select(c =>new ListProduct{
                                        Id= c.Id.ToString(),
                                        Name= c.Name,
                                         Stok = c.Stok,
                                         Price = c.Price,
                                         UpdatedDate = c.UpdatedDate,
                                         CreatedDate = c.CreatedDate,
                                        ProductImages=c.ProductImages.Select(a=> new ListProductImages
                                            {
                                                Id=a.Id.ToString(),
                                                FileName=a.FileName,
                                                Path=a.Path,
                                                Showcase=a.Showcase,
                                            }).ToList()
                                       }).ToList();

           return new ResponseProduct()
           { 
                TotalProductCount = totalProductCount,
                Products= products
            };
        }

        public async Task<byte[]> QrCodeToProductAsync(string productId)
        {
            Product product=await _productReadRepository.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Product bulunumadı");
            var plainOject = new
            {
                product.Id, product.Name, product.Stok, product.Price,product.CreatedDate
            };
            string plainText=JsonSerializer.Serialize(plainOject);
            return _qrCodeService.GenerateQRCode(plainText);
        }
    }
}
