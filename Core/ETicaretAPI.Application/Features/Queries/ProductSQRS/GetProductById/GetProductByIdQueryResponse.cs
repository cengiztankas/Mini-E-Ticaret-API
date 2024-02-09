using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.ProductSQRS.GetProductById
{
    public class GetProductByIdQueryResponse
    {
        public string Name { get; set; }
        public int Stok { get; set; }
        public decimal Price { get; set; }

    }
}
