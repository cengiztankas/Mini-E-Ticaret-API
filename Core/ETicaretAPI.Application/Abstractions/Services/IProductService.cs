using ETicaretAPI.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Services
{
    public interface IProductService
    {
        Task<ResponseProduct> GetProductsAsync(int size, int page);
        Task<byte[]> QrCodeToProductAsync(string productId);
    }
}
