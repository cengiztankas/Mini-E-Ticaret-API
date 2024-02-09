using ETicaretAPI.Application.DTOs.ProductImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.DTOs.Products
{
    public class ListProduct
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Stok { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<ListProductImages> ProductImages { get; set; }
    }
}
