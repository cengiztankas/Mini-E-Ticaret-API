using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.DTOs.ProductImage
{
    public class ListProductImages
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Id { get; set; }
        public bool Showcase { get; set; }
    }
}
