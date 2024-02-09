using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.ProductImageFileSQRS.GetProductImage
{
    public class GetProductImageQueryResponse
    {
        public string Path { get; set; }
        public string fileName { get; set; }
        public Guid Id { get; set; }
        public bool Showcase { get; set; }
    }
}
