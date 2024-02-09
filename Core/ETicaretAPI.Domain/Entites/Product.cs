using ETicaretAPI.Domain.Entites.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Domain.Entites
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int Stok { get; set; }
        public decimal Price { get; set; }
        public ICollection<ProductImageFile> ProductImages { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
    }
}  
