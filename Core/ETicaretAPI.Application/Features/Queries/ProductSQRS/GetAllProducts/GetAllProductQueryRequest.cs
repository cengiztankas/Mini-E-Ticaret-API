using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.ProductSQRS.GetAllProducts
{
    public class GetAllProductQueryRequest : IRequest<GetAllProductQueryResponse>
    {
        public int page { get; set; } = 0;
        public int size { get; set; } = 5;
    }
}
