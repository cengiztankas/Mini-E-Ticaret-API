using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFileSQRS.DeleteProductImage
{
    public class DeleteProductImageCommandRequest:IRequest<DeleteProductImageCommandResponse>
    {
        public string id { get; set; }
        public string? imageId { get; set; }
    }
}
